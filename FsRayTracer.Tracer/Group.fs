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
open RayTracer.Cylinder


module Group =

    let localIntersect object ray =

        let rec loop (objects: Object list) acc =
            match objects with
            | [] -> acc
            | h::t ->
                let intersections = Ray.intersect h ray
                loop t (acc @ intersections)

        let children =
            match object.shape with
            | Group l -> l
            | _ -> []

        loop children []
        |> List.sortBy (fun x -> x.t)

    let localNormalAt object worldPoint =
        failwith "group does not have localNormalAt"

    let getChildren objGroup =
        match objGroup.shape with
        | Group(l) -> l
        | _ -> failwith "expected object with shape group"

    let create () =
        { transform = Matrix.identityMatrix 4;
          transformInverse = Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = Group [];
          id = newRandom();
          localIntersect = localIntersect;
          localNormalAt = localNormalAt;
          parent = None }

    let setChildren children parent =
        { parent with shape = Group(children) }

    let addChildren children parent =
        match parent.shape with
        | Group g -> { parent with shape = Group(children @ g) }
        | _ -> failwith "expected a group"
            

