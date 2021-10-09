namespace RayTracer.Ray

open RayTracer.Point

open RayTracer.Vector
open System
open RayTracer.Intersection
open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Constnats
open RayTracer.Cone
open RayTracer.Cylinder
open RayTracer.Cube
open RayTracer.Helpers
open RayTracer.RayDomain
open RayTracer.ObjectDomain


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

        object.localIntersect object ray
