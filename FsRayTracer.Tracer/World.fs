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

[<AutoOpen>]
module Domain =
    type World = { light: Light; objects: Object list }

module World =

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
            Object.sphere
            |> Object.setMaterial material

        let s2 =
            Object.sphere
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

    let shadeHit (w:World) (c:Computation) =
        let shadowed = isShadowed c.overPoint w
        Object.lighting c.object.material w.light c.overPoint c.eyev c.normalv shadowed c.object

    let colorAt world ray =
        ray
        |> intersect world
        |> Intersection.hit
        |> fun hit ->
            match hit with
            | Some intersection ->
                intersection
                |> Computation.prepare ray
                |> shadeHit world
            | None -> Color.black

    let addObject object world =
        { world with objects = world.objects @ [object] }

    let setObjects objects world =
        { world with objects = objects }

    let withLight l world =
        { world with light = l }
