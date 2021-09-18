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
            

    // returns object with new child and object with new parent
    let add objectToAdd group =
        match group.shape with
        | Group l ->
            let uppdatedChild = { objectToAdd with parent = Some group }

            let newCildren = l @ [uppdatedChild]
            let group = { group with shape = Group(newCildren) }

            (uppdatedChild, group)
        | _ -> failwith "can only add object to Shape.Group objects"

    // returns object with new child and list of updated children with new parent
    let addList (objectsToAdd: Object list) (group:Object) =
        let rec loop objects groupAcc =
            match objects with
            | [] ->
                group
            | h::t ->
                let (group, child) = add h groupAcc
                loop t group

        loop objectsToAdd group

