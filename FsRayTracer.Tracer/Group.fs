namespace RayTracer.Group
open RayTracer.ObjectDomain
open RayTracer.Matrix
open RayTracer.Helpers
open RayTracer.RayDomain
open RayTracer.Vector
open RayTracer.Material
open RayTracer.Ray
open RayTracer.Object
open RayTracer.Sphere
open RayTracer.BoundingBox


module Group =

    let getChildren objGroup =  
        match objGroup.shape with
        | Group(l) -> l
        | _ -> failwith "expected object with shape group"

    let localIntersect object ray =

        match (BoundingBox.intersects ray object.bounds) with
        | true ->
            let rec loop (objects: Object list) acc =
                match objects with
                | [] -> acc
                | h::t ->
                    let intersections = Ray.intersect h ray
                    loop t (acc @ intersections)

            loop (getChildren object) [] |> List.sortBy (fun x -> x.t)
        | _ -> []

    let localNormalAt object worldPoint =
        failwith "group does not have localNormalAt"

    let create () =
        let t= 
            { transform = Matrix.identityMatrix 4;
              transformInverse = Matrix.identityMatrix 4 |> Matrix.inverse;
              material = Material.standard;
              shape = Group [];
              id = newRandom();
              localIntersect = localIntersect;
              localNormalAt = localNormalAt;
              bounds = BoundingBox.boundsOf (Group []) }
        t

    let setChildren children parent =
        { parent with shape = Group(children) }

    let addChildren children parent =
        match parent.shape with
        | Group g -> { parent with shape = Group(children @ g); bounds = BoundingBox.boundsOf (Group(children @ g))  }
        | _ -> failwith "expected a group"
            

