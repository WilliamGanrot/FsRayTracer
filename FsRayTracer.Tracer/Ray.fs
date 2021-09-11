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
open RayTracer.Cone
open RayTracer.Cylinder
open RayTracer.Cube
open RayTracer.Helpers

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
            object.transformInverse
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
        | Cylinder (min, max, _) ->
            let a = (ray.direction.X * ray.direction.X) + (ray.direction.Z * ray.direction.Z)

            match FloatHelper.equal 0. a with
            | true -> Cylinder.intersectCaps object ray.direction ray.origin []
            | false ->

                let b = (2. * ray.origin.X * ray.direction.X) + (2. * ray.origin.Z * ray.direction.Z)
                let c = (ray.origin.X * ray.origin.X) + (ray.origin.Z * ray.origin.Z) - 1.
                let disc = (b * b) - (4. * a * c)

                let t0 = ((-b) - Math.Pow(disc, 0.5)) / (2. * a)
                let t1 = ((-b) + Math.Pow(disc, 0.5)) / (2. * a)

                let (t0', t1') = if t0 > t1 then (t1, t0) else (t0, t1)

                let y0 = ray.origin.Y + t0' * ray.direction.Y
                let xs = if min < y0 && y0 < max then [Intersection.create object t0'] else []

                let y1 = ray.origin.Y + t1' * ray.direction.Y
                let xs' = if min < y1 && y1 < max then xs @ [Intersection.create object t1'] else xs

                Cylinder.intersectCaps object ray.direction ray.origin xs'
        | Cone (min, max, _) ->

            let a = Math.Pow(ray.direction.X, 2.) - Math.Pow(ray.direction.Y, 2.) + Math.Pow(ray.direction.Z, 2.)
            let b = (2. * ray.origin.X * ray.direction.X) - (2. * ray.origin.Y * ray.direction.Y) + (2. * ray.origin.Z * ray.direction.Z) 
            let c = (ray.origin.X * ray.origin.X) - (ray.origin.Y * ray.origin.Y) + (ray.origin.Z * ray.origin.Z)

            match (FloatHelper.equal a 0.), (FloatHelper.equal b 0.) with
            | true, true-> Cone.intersectCaps object ray.direction ray.origin []
            | true, false ->
                let t = -(c /. (2.*b))
                Cone.intersectCaps object ray.direction ray.origin [Intersection.create object t]
            | false, _ ->
                let disc = (b * b) - (4. * a * c)
                if disc < 0. then
                    []
                else

                    let t0 = (-b - Math.Sqrt(disc)) / (2. * a)
                    let t1 = (-b + Math.Sqrt(disc)) / (2. * a)

                    let (t0', t1') = if t0 > t1 then (t1, t0) else (t0, t1)

                    let y0 = ray.origin.Y + t0' * ray.direction.Y
                    let xs = if min < y0 && y0 < max then [Intersection.create object t0'] else []

                    let y1 = ray.origin.Y + t1' * ray.direction.Y
                    let xs' = if min < y1 && y1 < max then xs @ [Intersection.create object t1'] else xs

                    Cone.intersectCaps object ray.direction ray.origin xs'
            
                
                






