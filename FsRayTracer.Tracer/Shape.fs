namespace RayTracer.Shape

open System
open RayTracer.Transformation
open RayTracer.Matrix

[<AutoOpen>]
module Domain =
    type Sphere = {id:int; radii:float; transform: Matrix}

module Shape =

    let sphere =
        let r = new Random()
        {id = r.Next();
        radii = 1.;
        transform = Matrix.identityMatrix 4}

    let transform (t:Transformation) (s:Sphere) : Sphere =
        let t = Transformation.applyToMatrix t s.transform
        {s with transform = t}
