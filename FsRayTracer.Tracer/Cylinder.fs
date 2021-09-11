namespace RayTracer.Cylinder
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Object
open RayTracer.Helpers
open RayTracer.Intersection

module Cylinder =
    let checkCap (direction:Vector) (origin:Point) t =
        let x = origin.X + t * direction.X
        let z = origin.Z + t * direction.Z

        (x * x + z * z) <= 1.

    let intersectCaps (cyl:Object) (direction:Vector) (origin:Point) xs =

        match cyl.shape with
        | Cylinder (_, _, closed) when closed = false || (FloatHelper.equal direction.Y 0.) -> xs
        | Cylinder (min, max, closed) ->

            let t = (min - origin.Y) / direction.Y
            let xs' = if checkCap direction origin t then xs @ [Intersection.create cyl t] else xs

            let t' = (max - origin.Y) / direction.Y
            if checkCap direction origin t' then xs' @ [Intersection.create cyl t'] else xs'