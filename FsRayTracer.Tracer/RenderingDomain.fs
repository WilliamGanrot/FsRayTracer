namespace RayTracer.RenderingDomain
open RayTracer.RayDomain
open RayTracer.ObjectDomain

[<AutoOpen>]
module RenderingDomain =

    type Light = {poistion: Point; intensity: Color}
    type World = { light: Light; objects: Object list }

    //
    type ColorCordinat = {X: int; Y:int; Color:Color;}
    type Canvas = {Width: int; Height: int; Pixels: Color [,] }

    type Camera =
        { hsize: int;
          vsize: int;
          fov: float;
          transform: Matrix;
          transformInverse: Matrix;
          pixelSize: float;
          halfWidth: float;
          halfHeight: float }

