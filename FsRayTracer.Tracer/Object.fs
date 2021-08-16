namespace RayTracer.Object

open System
open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Material


[<AutoOpen>]
module Domain =
    type Sphere = { id: int; radii: float; }

    type Shape =
        Sphere of Sphere

    type Object =
        { transform: Matrix; material: Material; shape: Shape }

module Object =

    let sphere = 
        let r = new Random()

        let sphere = Sphere { id = r.Next(); radii = 1. }
        { transform = Matrix.identityMatrix 4; material = Material.standard; shape = sphere}

    let transform t object =
        let t = Transformation.applyToMatrix t object.transform
        {object with transform = t}

    let normal point object =

        let objectPoint =
            (Matrix.inverse object.transform)
            |> Matrix.multiplyPoint point

        let objectNormal = objectPoint - (Point.create 0. 0. 0.)

        let worldNormal =
            object.transform
            |> Matrix.inverse
            |> Matrix.Transpose
            |> Matrix.multiplyVector objectNormal

        Vector.normalize {worldNormal with W = 0.}

    let setMaterial m object =
        {object with material = m}
