namespace RayTracer.World

open RayTracer.Light
open RayTracer.Shape
open RayTracer.Point
open RayTracer.Color

[<AutoOpen>]
module Domain =
    type World = { light: Light Option; objects: Sphere list }

module World =

    let empty =
        { light = None;
          objects = [] }
          
    let ``default``() =
        { light = Some (Light.create (Color.create 1. 1. 1.) (Point.create -10. 10. -10.));
          objects = [] }