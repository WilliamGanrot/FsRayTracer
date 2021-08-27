namespace RayTracer.World

open RayTracer.Light
open RayTracer.Object
open RayTracer.Point
open RayTracer.Ray
open RayTracer.Color
open RayTracer.Material
open RayTracer.Transformation
open RayTracer.Computation
open RayTracer.Intersection
open RayTracer.Vector
open RayTracer.Helpers
open RayTracer.Constnats
open System

[<AutoOpen>]
module Domain =
    type World = { light: Light; objects: RayTracer.Object.Domain.Object list }

module World =

    let maxDepth = 4

    let empty =
        { light = Point.create -10. 10. -10. |> Light.create (Color.create 1. 1. 1.);
          objects = [] }
          
    let standard =
        let light =
            Point.create -10. 10. -10.
            |> Light.create Color.white

        let material =
            Material.standard
            |> Material.withColor (Color.create 0.8 1.0 0.6)
            |> Material.withDiffuse 0.7
            |> Material.withSpecular 0.2

        let s1 =
            Object.sphere()
            |> Object.setMaterial material

        let s2 =
            Object.sphere()
            |> Object.transform (Scaling(0.5, 0.5, 0.5))

        { light = light;
          objects = [s1;s2] }

    let intersect (w:World) r  = 
        w.objects
        |> List.map(fun x -> Ray.intersect x r)
        |> List.fold List.append []
        |> List.sort

    let isShadowed p w =
        let v = w.light.poistion - p
        let distance = Vector.magnitude v
        let direction = Vector.normalize v

        let optionHit =
            Ray.create p direction
            |> intersect w
            |> Intersection.hit

        match optionHit with
        | Some hit when hit.t < distance -> true
        |_ -> false

    let rec shadeHit (w:World) (remaining:int) (c:Computation) =
        let shadowed = isShadowed c.overPoint w

        let surface = Object.lighting c.object.material w.light c.overPoint c.eyev c.normalv shadowed c.object
        let reflected = reflectedColor c remaining w
        let refracted = refractedColor c remaining w

        let m = c.object.material
        match m.reflectivity > 0. && m.transparency > 0. with
        | true ->
            let reflactance = Computation.shlick c
            surface + (Color.mulitplyByScalar reflactance reflected) + Color.mulitplyByScalar (1. - reflactance) refracted 
        | false ->
            surface + reflected + refracted

    and colorAt world remaining ray =
        ray
        |> intersect world
        |> Intersection.hit
        |> fun hit ->
            match hit with
            | Some intersection ->
                intersection
                |> Computation.prepare ray [intersection]
                |> shadeHit world remaining
            | None -> Color.black
    
    and reflectedColor (comp:Computation) remaining world =
        match (comp.object.material.reflectivity, remaining) with
        | _, remaining when remaining <= 0 ->
            Color.black
        | reflectivenes, _ when reflectivenes < epsilon || -reflectivenes > epsilon ->
            Color.black
        | reflectivenes, _  ->
            let reflectiveRay = Ray.create comp.overPoint comp.reflectv
            let color = colorAt world (remaining - 1) reflectiveRay

            color |> Color.mulitplyByScalar reflectivenes

    and refractedColor (comps:Computation) remaining world =

        let noRemaining = remaining < 1
        let transparentMaterial = FloatHelper.equal comps.object.material.transparency 0.

        match (noRemaining, transparentMaterial) with
        | (true, _) | (_, true) -> Color.black
        | _ ->
            let nRatio = comps.n1 / comps.n2
            let cosI = Vector.dot comps.eyev comps.normalv
            let sin2T = (nRatio * nRatio) * (1. - (cosI * cosI))

            let cosT = Math.Sqrt(1. - sin2T)
            let direction = comps.normalv * ((nRatio * cosI) - cosT) - (comps.eyev * nRatio)

            let refactRay = Ray.create comps.underPoint direction

            let color =
                colorAt world (remaining - 1) refactRay
                |> Color.mulitplyByScalar comps.object.material.transparency

            color

    let addObject object world =
        { world with objects = world.objects @ [object] }

    let insertObject  object i world =
        { world with objects = insert object i world.objects }

    let setObjects objects world =
        { world with objects = objects }

    let withLight l world =
        { world with light = l }

