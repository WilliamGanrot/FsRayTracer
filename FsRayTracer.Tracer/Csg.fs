namespace RayTracer.Csg
open RayTracer.ObjectDomain
open RayTracer.Matrix
open RayTracer.Helpers
open RayTracer.Material
open RayTracer.Object
open RayTracer.Vector
open RayTracer.Ray


module Csg =

    let intersectionAllowed op lhit inl inr =
        match op with
        | Union -> (lhit && not inr) || (not lhit && not inl)
        | Intersect -> (lhit && inr) || (not lhit && inl)
        | Difference -> (lhit && not inr) || (not lhit && inl)

    let filterIntersections xs csg =
    
            let rec loop (left:Object) right operation (xs: Intersection list) inl inr result =
                match xs with
                | [] -> result
                | i::t ->
                    let lhit = Object.childIsIn left i.object
    
                    let result' =
                        match (intersectionAllowed operation lhit inl inr) with
                        | true -> result @ [i]
                        | false -> result
    
                    let inl', inr' =
                        match lhit with
                        | true -> (not inl), inr
                        | false -> inl, (not inr)
    
                    loop left right operation t inl' inr' result'
                
     
    
            match csg with
            | Csg (operation, left,right) -> loop left right operation xs false false []
            | _ -> failwith ""

    let localIntersect object ray =
        match object.shape with
        | Csg(_,left,right) ->
            let leftxs = Ray.intersect left ray
            let rightxs = Ray.intersect right ray

            let xs = leftxs @ rightxs |> List.sortBy (fun x -> x.t)
            filterIntersections xs object.shape

        | _ -> failwith ""

    let localNormalAt object worldPoint =
        failwith "csg does not have localNormalAt"


    let create (operation, o1, o2) =
        { transform = Matrix.identityMatrix 4;
          transformInverse= Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = Csg(operation, o1,o2);
          id = newRandom();
          localIntersect = localIntersect;
          localNormalAt = localNormalAt;
          bounds = None}

    
