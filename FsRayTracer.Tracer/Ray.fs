namespace RayTracer.Ray

open RayTracer.Point

open RayTracer.Vector
open System
open RayTracer.Shape
open RayTracer.Intersection
open RayTracer.Transformation
open RayTracer.Matrix

[<AutoOpen>]
module Domain =

    type Ray = {origin:Point; direction:Vector;}

module Ray =

    let create (p:Point) (v:Vector) : Ray =
        {origin = p; direction = v}

    let position (t:float) (r:Ray) : Point =
        r.origin + (r.direction * t)

    let transform (r:Ray) (t:Transformation) : Ray =
        let o = Transformation.applyToPoint t r.origin
        let d = Transformation.applyToVector t r.direction
        create o d

    let intersect sphere r =

        let ray =
            Matrix.inverse sphere.transform
            |> Matrix
            |> transform r

        let sphereToRay = ray.origin - Point.create 0. 0. 0.
        let a = Vector.dot ray.direction ray.direction
        let b = 2. * (Vector.dot ray.direction sphereToRay)
        let c = (Vector.dot sphereToRay sphereToRay) - 1.
        let discriminated = (b*b) - (4. * a * c)

        match discriminated with
        | d when d < 0. -> []
        | d ->
            let t1 = ((-b) - Math.Pow(discriminated, 0.5)) / (2. * a)
            let t2 = ((-b) + Math.Pow(discriminated, 0.5)) / (2. * a)

            let i1 = Intersection.create sphere t1
            let i2 = Intersection.create sphere t2
            [i1;i2]





