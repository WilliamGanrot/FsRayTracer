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

    let csg() =
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

        Csg.create (Difference, s, c)
        |> Object.transform (Scaling(0.7, 0.7, 0.7))
        |> Object.transform (Translation(0., 1.5, 1.))
        //|> Object.transform (Rotation(X, Math.PI/4.))
        |> Object.transform (Rotation(Y, Math.PI/6.))
        |> Object.transform (Rotation(X, Math.PI/6.))

    //let coverimage() =
        

    //let p =
    //    Plane.create()
    //    |> Object.setMaterial (
    //        Material.standard
    //        //|> Material.withPattern (Pattern.checkers (Color.white) (Color.black))
    //        |> Material.withReflectivity 0.5
    //        )

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



        


    let camera =
        Camera.create 1500 1500 0.785
        |> Camera.withTransfom (Transformation.viewTransform (Point.create -6. 6. -10.) (Point.create 6. 0. 6.) (Vector.create -0.45 1. 0.))

    let light = Light.create (Color.create 1. 1. 1.) (Point.create 50. 100. -50.)
    let light2 = Light.create (Color.create 0.2 0.2 0.2) (Point.create -400. 50. -10.)

    let whitematerial =
        Material.standard
        |> Material.withColor (Color.create 1. 1. 1.)
        |> Material.withDiffuse 0.7
        |> Material.withAmbient 0.1
        |> Material.withSpecular 0.
        |> Material.withReflectivity 0.1

    let bluematerial =
        whitematerial
        |> Material.withColor (Color.create 0.537 0.831 0.914)

    let redmaterial =
        whitematerial
        |> Material.withColor (Color.create 0.941 0.322 0.388)

    let purplematerial =
        whitematerial
        |> Material.withColor (Color.create 0.373 0.404 0.550)

    let standardtransform o =
        o
        |> Object.transform (Scaling(0.5, 0.5, 0.5))
        |> Object.transform (Translation(1., -1., 1.))
        

    let largeobject o =
        o
        |> Object.transform (Scaling(3.5,3.5,3.5))
        |> standardtransform
        

    
    let mediumobject o =
        o
        |> Object.transform (Scaling(3.5,3.5,3.5))
        |> standardtransform

    let smallobject o =
        o
        |> Object.transform (Scaling(2.,2.,2.))
        |> standardtransform
        
    let g1 = 
        let plane =
            Plane.create()
            |> Object.setMaterial (
                Material.standard
                |> Material.withColor (Color.create 1. 1. 1.)
                |> Material.withAmbient 1.
                |> Material.withDiffuse 0.
                |> Material.withSpecular 0.)
            |> Object.transform (Translation(0., 0., 500.))
            |> Object.transform (Rotation(X, Math.PI/2.))
        

        let sphere =
            Sphere.create()
            |> Object.setMaterial (
                Material.standard
                |> Material.withColor (Color.create 0.373 0.404 0.550)
                |> Material.withDiffuse 0.2
                |> Material.withAmbient 0.
                |> Material.withSpecular 1.
                |> Material.withShininess 200.
                |> Material.withReflectivity 0.7
                |> Material.WithrefractiveIndex 1.5)
            |> largeobject
        Group.create() |> Group.setChildren [plane;sphere]
    let g2 =
        let c1 =
            Cube.create()
            |> Object.setMaterial(whitematerial)
            |> Object.transform (Translation(4., 0., 0.))
            |> mediumobject
            

        let c2 =
            Cube.create()
            |> Object.setMaterial(bluematerial)
            |> Object.transform (Translation(8.5, 1.5, -0.5))
            |> largeobject
            

        let c3 =
            Cube.create()
            |> Object.setMaterial(redmaterial)
            |> Object.transform (Translation(0., 0., 4.))
            |> largeobject
            
        let c4 =
            Cube.create()
            |> Object.setMaterial(whitematerial)
            |> Object.transform (Translation(4., 0., 4.))
            |> smallobject
            

        let c5 =
            Cube.create()
            |> Object.setMaterial(purplematerial)
            |> Object.transform (Translation(7.5, 0.5, 4.))
            |> mediumobject
        Group.create() |> Group.setChildren [c1;c2;c3;c4;c5]
    let g3 =  
        let c6 =
            Cube.create()
            |> Object.setMaterial(whitematerial)
            |> Object.transform (Translation(-0.25, 0.25, 8.))
            |> mediumobject
            

        let c7 =
            Cube.create()
            |> Object.setMaterial (bluematerial)
            |> Object.transform (Translation(4., 1., 7.5))
            |> largeobject

        let c8 =
            Cube.create()
            |> Object.setMaterial (redmaterial)
            |> Object.transform (Translation(10., 2., 7.5))
            |> mediumobject

        let c9 =
            Cube.create()
            |> Object.setMaterial (whitematerial)
            |> Object.transform (Translation(8., 2., 12.))
            |> smallobject



        let c10 =
            Cube.create()
            |> Object.setMaterial (whitematerial)
            |> Object.transform (Translation(20., 1., 9.))
            |> smallobject
        Group.create() |> Group.setChildren [c6;c7;c8;c9;c10]
    let g4 =
        let c11 =
            Cube.create()
            |> Object.setMaterial (bluematerial)
            |> Object.transform (Translation(-0.5, -5., 0.25))
            |> largeobject

        let c12 =
            Cube.create()
            |> Object.setMaterial (redmaterial)
            |> Object.transform (Translation(4., -4., 0.))
            |> largeobject

        let c13 =
            Cube.create()
            |> Object.setMaterial (whitematerial)
            |> Object.transform (Translation(8.5, -4., 0.))
            |> largeobject

        let c14 =
            Cube.create()
            |> Object.setMaterial (whitematerial)
            |> Object.transform (Translation(0., -4., 4.))
            |> largeobject

        let c15 =
            Cube.create()
            |> Object.setMaterial (purplematerial)
            |> Object.transform (Translation(-0.5, -4.5, 8.))
            |> largeobject
        let c16 =
            Cube.create()
            |> Object.setMaterial (whitematerial)
            |> Object.transform (Translation(0., -8., 4.))
            |> largeobject
        let c17 =
            Cube.create()
            |> Object.setMaterial (whitematerial)
            |> Object.transform (Translation(-0.5, -8.5, 8.))
            |> largeobject
        Group.create() |> Group.setChildren [c11;c12;c13;c14;c15;c16;c17]

    let objectsgroup =
        Group.create()
        |> Group.setChildren [g1;g2;g3;g4]
    let world =
        World.empty
        |> World.addObject objectsgroup
        |> World.withLights [light; light2]

    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let ppm =
        camera
        |> Camera.render world
        |> Canvas.toPPM
    stopWatch.Stop()

    System.IO.File.WriteAllText ("image.pgm", ppm)

    0    