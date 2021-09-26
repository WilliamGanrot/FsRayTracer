namespace RayTracer.Triangle
open RayTracer.ObjectDomain
open RayTracer.Matrix
open RayTracer.Helpers
open RayTracer.Material
open RayTracer.Vector
open RayTracer.RayDomain
open RayTracer.Constnats
open RayTracer.Intersection

module Triangle =

    let localIntersect object (ray:Ray) =
        match object.shape with
        | Traingle(p1,_,_,e1,e2,n) ->
            let dirrCrossE2 = Vector.cross ray.direction e2
            let det = Vector.dot e1 dirrCrossE2

            match abs(det) < epsilon with
            | true -> []
            | false ->
                let f = 1. / det

                let p1ToOrigin = ray.origin - p1
                let u = f * (Vector.dot p1ToOrigin dirrCrossE2)
                
                let originCrossE1 = Vector.cross p1ToOrigin e1
                let v = f * (Vector.dot ray.direction originCrossE1)

                match u,v with
                | u,_ when u < 0. || u > 1. -> []
                | u,v when v < 0. || (u + v) > 1. ->  []
                | _ ->
                    let t = f * (Vector.dot e2 originCrossE1)
                    [Intersection.create object t]

        | _ -> failwith "expected traingle"

    let localNormalAt shape point =
        match shape with
        | Traingle(_,_,_,_,_,n) -> n
        | _ -> failwith "expected traingle"

    let create((p1:Point), (p2:Point), (p3:Point)) =

        let e1 = p2 - p1 
        let e2 = p3 - p1
        let n = Vector.cross e2 e1 |> Vector.normalize

        { transform = Matrix.identityMatrix 4;
          transformInverse= Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = Traingle(p1, p2, p3, e1, e2, n);
          id = r.Next();
          localIntersect = localIntersect;
          localNormalAt = localNormalAt;
          parent = None }

