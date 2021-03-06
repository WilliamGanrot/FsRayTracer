namespace RayTracer.World

open RayTracer.Light
open RayTracer.Object
open RayTracer.Point
open RayTracer.Ray
open RayTracer.Color
open RayTracer.Material
open RayTracer.Transformation
open RayTracer.Intersection
open RayTracer.Vector
open RayTracer.Helpers
open RayTracer.Constnats
open RayTracer.Computation
open RayTracer.RenderingDomain
open RayTracer.ObjectDomain
open RayTracer.Sphere
open System

module World =

    let maxDepth = 4

    let empty =
        { lights = [Light.create (Color.create 1. 1. 1.) (Point.create -10. 10. -10.)];
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
            Sphere.create()
            |> Object.setMaterial material

        let s2 =
            Sphere.create()
            |> Object.transform (Scaling(0.5, 0.5, 0.5))

        { lights = [light];
          objects = [s1;s2] }

    let intersect (w:World) r = 
        w.objects
        |> List.map(fun x -> Ray.intersect x r)
        |> List.fold List.append []
        |> List.sortBy (fun x -> x.t)

    let isShadowed fromPoint toPoint w =
        let v = fromPoint - toPoint
        let distance = Vector.magnitude v
        let direction = Vector.normalize v

        let optionHit =
            Ray.create toPoint direction
            |> intersect w
            |> Intersection.hit

        match optionHit with
        | Some hit when hit.t < distance -> true
        |_ -> false

    let rec shadeHit (w:World) (remaining:int) (c:Computation) =

        let rec loopLights lights colorList =
            match lights with
            | [] ->
                let head = colorList |> List.head
                let tail = colorList |> List.tail
                tail |> List.fold (fun c c2 -> c + c2) head
            | light::xs ->
                let shadowed = isShadowed light.poistion c.overPoint w

                let surface = Object.lighting c.object.material light c.overPoint c.eyev c.normalv shadowed c.object
                let reflected = reflectedColor c remaining w
                let refracted = refractedColor c remaining w

                let m = c.object.material
                match m.reflectivity > 0. && m.transparency > 0. with
                | true ->
                    let reflactance = Computation.shlick c
                    let color = surface + (Color.mulitplyByScalar reflactance reflected) + Color.mulitplyByScalar (1. - reflactance) refracted
                    loopLights xs (colorList @ [color])
                | false ->
                    let color = surface + reflected + refracted
                    loopLights xs (colorList @ [color])

        loopLights w.lights []


    and colorAt world remaining ray =
        ray
        |> intersect world
        |> Intersection.hit
        |> fun hit ->
            match hit with
            | Some intersection ->
                intersection
                |> Computation.prepare ray [intersection] world.objects
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

    let addObject (object) (world:World) =
        { world with objects = world.objects @ [object] }

    let insertObject  object i (world:World) =
        { world with objects = insert object i world.objects }

    let setObjects objects (world:World) =
        { world with objects = objects }

    let withLights l world =
        { world with lights = l }

