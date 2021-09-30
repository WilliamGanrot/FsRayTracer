namespace RayTracer.Cylinder
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Matrix
open RayTracer.Helpers
open RayTracer.Intersection
open RayTracer.Material
open RayTracer.RayDomain
open RayTracer.ObjectDomain
open System
open RayTracer.Constnats

module Cylinder =
    let checkCap (direction:Vector) (origin:Point) t =
        let x = origin.X + t * direction.X
        let z = origin.Z + t * direction.Z

        (x * x + z * z) <= 1.

    let intersectCaps cyl (direction:Vector) (origin:Point) xs =

        match cyl.shape with
        | Cylinder (_, _, closed) when closed = false || (FloatHelper.equal direction.Y 0.) -> xs
        | Cylinder (min, max, closed) ->

            let t = (min - origin.Y) / direction.Y
            let xs' = if checkCap direction origin t then xs @ [Intersection.create cyl t] else xs

            let t' = (max - origin.Y) / direction.Y
            if checkCap direction origin t' then xs' @ [Intersection.create cyl t'] else xs'
        | _ -> failwith "invalid shape, expected a cylinder"

    let localIntersect object ray =
        match object.shape with
        | Cylinder(min, max, _) ->
            let a = (ray.direction.X * ray.direction.X) + (ray.direction.Z * ray.direction.Z)

            match FloatHelper.equal 0. a with
            | true -> intersectCaps object ray.direction ray.origin []
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

                intersectCaps object ray.direction ray.origin xs'
        | _ -> failwith "invalid shape"

    let localNormalAt shape objectPoint =
        match shape with
        | Cylinder(min, max, _) ->
            let dist = objectPoint.X * objectPoint.X + objectPoint.Z * objectPoint.Z

            match dist < 1. with
            | true when objectPoint.Y >= max - epsilon -> Vector.create 0. 1. 0.
            | true when objectPoint.Y <= min + epsilon -> Vector.create 0. -1. 0.
            | _ -> Vector.create objectPoint.X 0. objectPoint.Z
        | _ -> failwith "invalid shape, expected cylinder"

    let build(cyl) =
        { transform = Matrix.identityMatrix 4;
          transformInverse= Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = cyl;
          id = newRandom();
          localIntersect = localIntersect;
          localNormalAt = localNormalAt;
          bounds = None}

    let create() = build (Cylinder(-infinity, infinity, false))

    
