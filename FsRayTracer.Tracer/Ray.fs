namespace RayTracer.Ray

open RayTracer.Point

open RayTracer.Vector
open System
open RayTracer.Object
open RayTracer.Intersection
open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Constnats
open RayTracer.Object

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

    let intersect (object:Object) r =

        let ray =
            Matrix.inverse object.transform
            |> Matrix
            |> transform r

        match object.shape with
        | Sphere sphere ->

            let sphereToRay = ray.origin - Point.create 0. 0. 0.
            let a = Vector.dot ray.direction ray.direction
            let b = 2. * (Vector.dot ray.direction sphereToRay)
            let c = (Vector.dot sphereToRay sphereToRay) - 1.
            let discriminated = (b*b) - (4. * a * c)

            match discriminated with
            | d when d < 0. -> []
            | _ ->
                let t1 = ((-b) - Math.Pow(discriminated, 0.5)) / (2. * a)
                let t2 = ((-b) + Math.Pow(discriminated, 0.5)) / (2. * a)

                let i1 = Intersection.create object t1
                let i2 = Intersection.create object t2
                [i1;i2]
        | Plane ->
            match Math.Abs(ray.direction.Y) < epsilon with
            | true  -> []
            | false ->
                let i =
                    let t = -(ray.origin.Y / ray.direction.Y)
                    Intersection.create object t
                [i]
        | Cube ->

            let (xtmin, xtmax) = Cube.checkAxis ray.origin.X ray.direction.X
            let (ytmin, ytmax) = Cube.checkAxis ray.origin.Y ray.direction.Y
            let (ztmin, ztmax) = Cube.checkAxis ray.origin.Z ray.direction.Z

            let tmin = [xtmin; ytmin; ztmin] |> List.max
            let tmax = [xtmax; ytmax; ztmax] |> List.min

            match tmin > tmax with
            | true -> []
            | false ->
                let i1 = Intersection.create object tmin
                let i2 = Intersection.create object tmax
            
                [i1; i2]
            
                
                






