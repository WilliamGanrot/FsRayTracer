namespace RayTracer.Domain
open RayTracer.Helpers


[<AutoOpen>]
module Domain =
    0
    //



    //


    //


    //
    //type PatternType =
    //    | Stripes
    //    | Gradient
    //    | Rings
    //    | Checkers
    //type Pattern = { transform: Matrix; patternType: PatternType; a: Color; b: Color; }

    //type Material =
    //    { color: Color;
    //      ambient:float;
    //      diffuse:float;
    //      specular: float;
    //      shininess: float;
    //      pattern: Pattern Option;
    //      reflectivity: float;
    //      transparency: float;
    //      refractiveIndex: float; }

    //      static member (.=) (m : Material, m2: Material) =
    //        m.color .= m2.color &&
    //        FloatHelper.equal m.ambient m2.ambient &&
    //        FloatHelper.equal m.diffuse m2.diffuse &&
    //        FloatHelper.equal m.specular m2.shininess &&
    //        FloatHelper.equal m.reflectivity m2.reflectivity &&
    //        FloatHelper.equal m.transparency m2.transparency &&
    //        FloatHelper.equal m.refractiveIndex m2.refractiveIndex &&
    //        m.pattern = m2.pattern

    //type Shape =
    //    | Sphere
    //    | Plane
    //    | Cube
    //    | Cylinder of minimum: float * maximum: float * closed: bool
    //    | Cone of minimum: float * maximum: float * closed: bool
    //type Object =
    //    { transform: Matrix; transformInverse: Matrix; material: Material; shape: Shape; id: int}

    //    //compares object without id
    //     static member (.=.) (p, v : Object) =
    //        {p with id = 0} = {v with id = 0}

    //type Computation =
    //    { t: float;
    //      object: Object;
    //      point:Point;
    //      overPoint: Point;
    //      eyev: Vector;
    //      inside: bool;
    //      normalv: Vector;
    //      reflectv: Vector;
    //      n1: float;
    //      n2: float;
    //      underPoint: Point}

    //type Intersection = { t: float; object: Object }

    //
    //type Light = {poistion: Point; intensity: Color}
    //type World = { light: Light; objects: Object list }

    ////
    //type ColorCordinat = {X: int; Y:int; Color:Color;}
    //type Canvas = {Width: int; Height: int; Pixels: Color [,] }

    //type Camera =
    //    { hsize: int;
    //      vsize: int;
    //      fov: float;
    //      transform: Matrix;
    //      transformInverse: Matrix;
    //      pixelSize: float;
    //      halfWidth: float;
    //      halfHeight: float }
