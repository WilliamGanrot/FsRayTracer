namespace RayTracer.Cone
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Object
open RayTracer.Helpers
open RayTracer.Intersection
open RayTracer.Object
open RayTracer.Matrix
open RayTracer.Material


module Cone =

    //let create =
    //    { transform = Matrix.identityMatrix 4;
    //      transformInverse= Matrix.identityMatrix 4 |> Matrix.inverse;
    //      material = Material.standard;
    //      shape = Cone(-infinity, infinity, false);
    //      id = r.Next(); }


    let checkCap (direction:Vector) (origin:Point) t y =
        let x = origin.X + t * direction.X
        let z = origin.Z + t * direction.Z
    
        (x * x + z * z) <= y * y
    
    let intersectCaps (cyl:Object) (direction:Vector) (origin:Point) xs =
    
        match cyl.shape with
        | Cone (_, _, closed) when closed = false || (FloatHelper.equal direction.Y 0.) -> xs
        | Cone (min, max, closed) ->
    
            let t = (min - origin.Y) / direction.Y
            let xs' = if checkCap direction origin t min then (Intersection.create cyl t) :: xs else xs
    
            let t' = (max - origin.Y) / direction.Y
            if checkCap direction origin t' max then (Intersection.create cyl t') :: xs' else xs'

