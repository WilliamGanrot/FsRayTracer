open RayTracer.Vector
open RayTracer.Point
open RayTracer.Color
open System
open Raytracer.Canvas
open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Material
open RayTracer.Object
open RayTracer.Intersection
open RayTracer.Ray
open RayTracer.Light
open RayTracer.Cone
open RayTracer.World
open RayTracer.Camera
open RayTracer.Pattern
open RayTracer.Group
open RayTracer.Plane
open RayTracer.ObjectDomain
open RayTracer.Cylinder
open RayTracer.Sphere

type Node = Node of value:int * children:Node list
type Node2 = Node2 of value:int * parent: Node2 Option


[<EntryPoint>]
let main argv =
  


    
    let createHexagon() =

        let createHexagonEdge() =
            { Cylinder.create() with shape = Cylinder(0., 1., false) }
            |> Object.transform (Translation(0., 0., -1.))
            |> Object.transform (Rotation(Y, -Math.PI/6.))
            |> Object.transform (Rotation(Z, -Math.PI/2.))
            |> Object.transform (Scaling(0.25, 1., 0.25))


        let createHexagonCorner() =
            Sphere.create()
            |> Object.transform (Translation(0., 0., -1.))
            |> Object.transform (Scaling(0.25, 0.25, 0.25))

        let createHexagonSide() =
            let g = Group.create()

            let hc1 = createHexagonCorner()
            let hc2 = createHexagonEdge()
            Group.setChildren [hc1;hc2] g

        let hex = Group.create()





        let sides =
            [ for n in [0..5] do
                let side =
                    createHexagonSide()
                    |> Object.transform (Rotation(Y, ((float n) * -Math.PI)/3.))
                side ]

            //let (_, hex') = Group.add side hex
            //hex <- hex'
        Group.setChildren sides hex


    let p =
        Plane.create()
        |> Object.setMaterial (
            Material.standard
            |> Material.superShiny
            |> Material.withDiffuse 0.7
            |> Material.withSpecular 1.
            |> Material.withShininess 300.
            |> Material.withReflectivity 0.5
            |> Material.WithrefractiveIndex 1.5)

    //let cone =
    //    { Cone.create() with shape = Cone(0., 1., true) }
    //    |> Object.setMaterial(
    //        Material.standard
    //        |> Material.withColor (Color.create 0.8 1. 0.7)
    //        |> Material.superShiny
    //        |> Material.withDiffuse 0.7
    //        |> Material.withSpecular 1.
    //        |> Material.withShininess 300.
    //        |> Material.withReflectivity 0.3
    //        |> Material.WithrefractiveIndex 1.5)
    //    |> Object.transform (Translation(1.7, 1.5, 2.5))
    //    |> Object.transform (Rotation(Z,(Math.PI/2.)))
    //    //|> Object.transform (Scaling(0.5, 0.5, 0.5))


    
    //let cyl =
    //    { Cylinder.create() with shape = Cylinder(0., 1., true) }
    //    |> Object.setMaterial (

    //        Material.standard

    //        |> Material.superShiny
    //        |> Material.withColor (Color.create 0.7 0. 0.3)
    //        //|> Material.withDiffuse 0.7
    //        |> Material.withReflectivity 0.4 )
    //    |> Object.transform (Translation(-1.8, 1.5, 2.5))
    //    |> Object.transform (Rotation(Z,-(Math.PI/2.)))
    //    |> Object.transform (Scaling(0.5, 2.5, 1.))
            
        //|> Object.transform (Translation(2., 0.3, 0.5))
        //|> Object.transform (Rotation(Y,-(Math.PI/4.5)))
        //|> Object.transform (Scaling(2., 0.3, 0.5))
  
    //let sphere =
    //    Sphere.create()
    //    |> Object.transform (Translation(0., 0.8, 2.5))
    //    |> Object.transform (Scaling(0.8, 0.8, 0.8))
        
        
    //    |> Object.setMaterial(
    //        Material.standard
    //        |> Material.withDiffuse 0.7
    //        |> Material.withSpecular 1.
    //        |> Material.withShininess 300.
    //        |> Material.withReflectivity 0.9
    //        |> Material.superShiny
    //        |> Material.withColor (Color.black))



    let tree = [
        Node(1,
            [Node(3,
                  [])
            ]);
        Node(2,
             [
                 Node(4,
                    [
                        Node(6,[])
                    ]
                    );
                 Node(5,
                 [
                    Node(7,[]);
                    Node(8,[])
                 ])
             ])
    
        ]
    let tree2 =
        let n2 =
            let n2 = Node2(2,None)

            let n4 = Node2(4,Some n2)
            let n6 = Node2(6,Some n4)

            let n5 = Node2(5,Some n2)
            let n8 = Node2(8,Some n5)
            let n7 = Node2(7,Some n5)
            n2
        let n1 =
            let n1 = Node2(1,None)

            let n3 = Node2(3,Some n1)
            n1
        [n1; n2]
    //let findintree (key:int) (tree:Node list) : Node Option =
    //    let rec loopNode (node: Node) =
    //        printfn "%A" node |> ignore
    //        match node with
    //        | Node(v,_) when v = key ->
    //            Some node
    //        | Node(v,children) when children.Length <> 0 -> loopChildren children
    //        | _ -> None
            
    
    //    and loopChildren (tree: Node list) =
    //        match tree with
    //        | h::t ->
    //            match loopNode h with
    //            | Some v ->
    //                printfn "%A" h
    //                Some h
    //            | None -> loopChildren t 

    //        | _ -> None

    //    loopChildren tree

    //let v = findintree 3 tree
    let gettrail (key:int) (tree:Node list)=
        let mutable mlist = []

        let rec loopNode (node: Node) =
                match node with
                | Node(v,_) when v = key ->
                    Some node
                | Node(v,children) when children.Length <> 0 -> loopChildren children
                | _ -> None
            
    
        and loopChildren (tree: Node list) =
            match tree with
            | h::t ->
                match loopNode h with
                | Some v ->
                    mlist <- mlist @ [h]
                    Some v
                | None -> loopChildren t 

            | _ -> None

        loopChildren tree |> ignore
        mlist

    let c = gettrail 8 tree
        //match trail with
        //| [] -> Matrix.multiplyPoint point object.transformInverse
        //| _ -> trail |> List.fold (fun p o -> Matrix.multiplyPoint p o.transformInverse) point


    let x =
        createHexagon()
        |> Object.transform(Rotation(X, -Math.PI/6.))
        |> Object.transform(Translation(0., 1., 0.))
        

    let world =
        World.empty
        |> World.addObject x
        //|> World.addObject p
        //|> World.addObject cone
        //|> World.addObject cyl
        //|> World.addObject sphere
        //|> World.addObject glass
        |> World.withLight (
            Light.create (Color.create 1. 1. 1.) (Point.create -10. 10. -10.))

    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let ppm =
        Camera.create 400 200 (Math.PI/3.)
        |> Camera.withTransfom (Transformation.viewTransform (Point.create 0. 1.5 -5.) (Point.create 0. 1. 0.) (Vector.create 0. 1. 0.))
        |> Camera.render world
        |> Canvas.toPPM
    stopWatch.Stop()

    System.IO.File.WriteAllText ("image.pgm", ppm)

    0    