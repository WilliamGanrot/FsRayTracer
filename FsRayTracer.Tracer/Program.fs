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
open RayTracer.Csg
open RayTracer.Cube
open RayTracer.OBJFile
open System
open RayTracer.Color
open RayTracer.Color

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
            //|> Material.withPattern (Pattern.checkers (Color.white) (Color.black))
            |> Material.withReflectivity 0.5
            )

    //let parser =
    //        let f =
    //            File.ReadAllLines "../../../../OBJModels/teapot.obj"
    //            |> Array.map(fun x -> x.TrimEnd())
    //        //let folded = f |> Array.fold (fun r s -> r + s + "\n") ""
    //        OBJFile.parseFile f


    //let g =

    //    let lg=
    //        parser.defaultGroup
    //        |> List.map (fun x -> x |> Object.setMaterial (Material.standard |> Material.withColor (Color.create 0.1 0.5 0.3)))
    //        |> List.chunkBySize 250 
    //        |> List.map (fun x -> Group.create() |> Group.setChildren x)
            

    //    Group.create()
    //    |> Group.setChildren lg
    //    |> Object.transform (Rotation(Y, Math.PI/5.))
    //    |> Object.transform (Translation(-3.5,-2.6, 4.5))
    //    |> Object.transform (Scaling(1.5,1.5,1.5))


    let s =
        Sphere.create()
        |> Object.setMaterial(
            Material.standard
            |> Material.withDiffuse 0.7
            |> Material.withSpecular 1.
            |> Material.withShininess 300.
            |> Material.withReflectivity 0.3
            |> Material.withColor (Color.create 0.7 0. 0.2))
        |> Object.transform (Scaling(1.3, 1.3, 1.3))

    let c =
        Cube.create()
        |> Object.setMaterial (
            Material.standard
            |> Material.withDiffuse 0.7
            |> Material.withSpecular 1.
            |> Material.withShininess 300.
            |> Material.withReflectivity 0.3
            |> Material.withColor (Color.create 0. 1. 1.))
        
    let csg =
        Csg.create (Difference, s, c)
        |> Object.transform (Scaling(0.7, 0.7, 0.7))
        |> Object.transform (Translation(0., 1.5, 1.))
        //|> Object.transform (Rotation(X, Math.PI/4.))
        |> Object.transform (Rotation(Y, Math.PI/6.))
        |> Object.transform (Rotation(X, Math.PI/6.))


    let world =
        World.empty
        |> World.addObject csg
        |> World.addObject p
        //
        //|> World.addObject csg2
        //|> World.addObject (parser.topGroup |> Object.transform (Translation(0.,0., 3.)))
        |> World.withLight (
            Light.create (Color.create 1. 1. 1.) (Point.create -10. 10. -10.))

    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let ppm =
        Camera.create 1200 600 (Math.PI/3.)
        |> Camera.withTransfom (Transformation.viewTransform (Point.create 0. 1.5 -5.) (Point.create 0. 1. 0.) (Vector.create 0. 1. 0.))
        |> Camera.render world
        |> Canvas.toPPM
    stopWatch.Stop()

    System.IO.File.WriteAllText ("image.pgm", ppm)

    0    