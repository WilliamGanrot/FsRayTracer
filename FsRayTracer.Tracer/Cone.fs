namespace RayTracer.Cone
open RayTracer.Vector
open RayTracer.Point

open RayTracer.Helpers
open RayTracer.Intersection

open RayTracer.Matrix
open RayTracer.Material
open RayTracer.RayDomain
open RayTracer.ObjectDomain
open System
open RayTracer.Constnats


module Cone =

    let checkCap (direction:Vector) (origin:Point) t y =
        let x = origin.X + t * direction.X
        let z = origin.Z + t * direction.Z
    
        (x * x + z * z) <= y * y
    
    let intersectCaps cyl (direction:Vector) (origin:Point) xs =
    
        match cyl.shape with
        | Cone (_, _, closed) when closed = false || (FloatHelper.equal direction.Y 0.) -> xs
        | Cone (min, max, closed) ->
    
            let t = (min - origin.Y) / direction.Y
            let xs' = if checkCap direction origin t min then (Intersection.create cyl t) :: xs else xs
    
            let t' = (max - origin.Y) / direction.Y
            if checkCap direction origin t' max then (Intersection.create cyl t') :: xs' else xs'
        | _ -> failwith "invalid shape, expected a cone"

    let localIntersect object ray =
        
        match object.shape with
        | Cone(min, max, _) ->
            let a = Math.Pow(ray.direction.X, 2.) - Math.Pow(ray.direction.Y, 2.) + Math.Pow(ray.direction.Z, 2.)
            let b = (2. * ray.origin.X * ray.direction.X) - (2. * ray.origin.Y * ray.direction.Y) + (2. * ray.origin.Z * ray.direction.Z) 
            let c = (ray.origin.X * ray.origin.X) - (ray.origin.Y * ray.origin.Y) + (ray.origin.Z * ray.origin.Z)

            match (FloatHelper.equal a 0.), (FloatHelper.equal b 0.) with
            | true, true-> intersectCaps object ray.direction ray.origin []
            | true, false ->
                let t = -(c /. (2.*b))
                intersectCaps object ray.direction ray.origin [Intersection.create object t]
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

                    intersectCaps object ray.direction ray.origin xs'
        | _ ->
            let x = object.shape
            let y = x
            failwith "invalid shape"

    let localNormalAt shape objectPoint i =
        match shape with
        | Cone (min, max, _) ->
            let dist = objectPoint.X * objectPoint.X + objectPoint.Z * objectPoint.Z

            match dist < 1. with
            | true when objectPoint.Y >= max - epsilon -> Vector.create 0. 1. 0.
            | true when objectPoint.Y <= min + epsilon -> Vector.create 0. -1. 0.
            | _ ->
                let y = Math.Sqrt(objectPoint.X * objectPoint.X + objectPoint.Z * objectPoint.Z)
                let y' = if objectPoint.Y > 0. then -y else y
                Vector.create objectPoint.X y' objectPoint.Z
        | _ -> failwith "invalid shape, expected cone"

    let build cone =
        { transform = Matrix.identityMatrix 4;
          transformInverse= Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = cone;
          id = r.Next();
          localIntersect = localIntersect;
          localNormalAt = localNormalAt;
          bounds = None }
    
    let create() = build (Cone(-infinity, infinity, false))


    