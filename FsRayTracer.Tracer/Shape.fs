namespace RayTracer.Shape

open System
open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Vector
open RayTracer.Point


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

    let normal (p:Point) (s:Sphere) =

        let objectPoint =
            (Matrix.inverse s.transform)
            |> Matrix.multiplyPoint p

        let objectNormal = objectPoint - (Point.create 0. 0. 0.)

        let worldNormal =
            s.transform
            |> Matrix.inverse
            |> Matrix.Transpose
            |> Matrix.multiplyVector objectNormal

        Vector.normalize {worldNormal with W = 0.}
