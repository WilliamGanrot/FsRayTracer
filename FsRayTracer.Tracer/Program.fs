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
open System.IO


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

    let parser =
            let f =
                File.ReadAllLines "../../../../OBJModels/dragon.obj"
                |> Array.map(fun x -> x.TrimEnd())
            OBJFile.parseFile f

    let g =
        Group.create() |> Group.setChildren parser.defaultGroup |> Object.divide 50

    let world =
        World.empty
        |> World.addObject g
        |> World.addObject (Plane.create())
        |> World.withLights (
            [Light.create (Color.create 1. 1. 1.) (Point.create -10. 10. -10.)])

    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let ppm =

        Camera.create 50 25 (Math.PI/3.)
        |> Camera.withTransfom (Transformation.viewTransform (Point.create 0. 3.5 -8.) (Point.create 0. 1. 0.) (Vector.create 0. 1. 0.))
        |> Camera.render world
        |> Canvas.toPPM
    stopWatch.Stop()

    System.IO.File.WriteAllText ("image.pgm", ppm)

    0    