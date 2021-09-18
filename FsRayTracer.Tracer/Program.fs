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


[<EntryPoint>]
let main argv =
    (*IMAGE 1*)
    (*
    let tick environment projectile =
        { position = projectile.position + projectile.velocity;
          velocity = projectile.velocity + environment.gravity + environment.wind }

    let p =
        { position = Point.create 0. 1. 0.;
          velocity = (Vector.create 1.0 1.8 0.0 |> Vector.normalize) * 11.25 }

    let e =
        { gravity = Vector.create 0.0 -0.1 0.0;
          wind    = Vector.create -0.01 0.0 0.0 }

    let c = Canvas.makeCanvas 900 550

    let rec getProjectiles env proj list =
        let newProjectile = tick env proj

        match newProjectile.position with
        | p when p.Y > 0.0 ->
            list @ [newProjectile] |> getProjectiles e newProjectile
        | p ->
            list

    let list = getProjectiles e p [p]

    list
    |> List.iter(fun p ->
        Canvas.setPixel (int p.position.X) (c.Height - (int p.position.Y)) Color.red c
        p |> ignore)

    let ppm = Canvas.toPPM c
    System.IO.File.WriteAllText("image.pgm", ppm);
    *)


    (*IMAGE 2*)
    (*
        let c = Canvas.makeCanvas 400 400

        let angle = (Math.PI)/6.
        let radius = 3./8. * float c.Width

        let twelwP = Point.create 0. 0. 1.


        [1..12]
        |> List.iter(fun h ->
            let p =
                twelwP
                |> Transformation.applyToPoint (Rotation(Y, (float h) * angle))
                |> Transformation.applyToPoint (Scaling(radius, 0., radius))
                |> Transformation.applyToPoint (Translation((float c.Width/2.), 0., (float c.Width/2.)))

            c |> Canvas.setPixel (int p.X) (c.Height-(int p.Z)) Color.red)


        let ppm = Canvas.toPPM c
        System.IO.File.WriteAllText("image.pgm", ppm);
    *)

    (*IMAGE 3*)
    (*
    let rayOrigin = Point.create 0. 0. -5.
    let wallZ = 10
    let wallSize = 7.
    let canvasPixels = 100
    let pixelSize = wallSize/ (float canvasPixels)
    let half = wallSize / 2.
    let canvas = Canvas.makeCanvas canvasPixels canvasPixels
    let shape = Shape.sphere

    let hits x y =
        let worldY = half - pixelSize * (float y)
        let worldX = -half + pixelSize * (float x)
        let position = Point.create worldX worldY (float wallZ)

        let hit =
            Vector.normalize (position - rayOrigin)
            |> Ray.create rayOrigin 
            |> Ray.intersect shape
            |> Intersection.hit

        match hit with
        | Some _ -> Some {X = x; Y = y}
        | _ -> None

    canvas
    |> Canvas.cordinats
    |> List.map (fun c -> hits c.X c.Y)
    |> List.choose (fun x -> x)
    |> List.iter(fun c -> Canvas.setPixel c.X c.Y Color.red canvas)

    let ppm = Canvas.toPPM canvas
    System.IO.File.WriteAllText ("image.pgm", ppm)
    *)

   (*image 4

   let rayOrigin = Point.create 0. 0. -5.
      let wallZ = 10
      let wallSize = 7.
      let canvasPixels = 800
      let pixelSize = wallSize/ (float canvasPixels)
      let half = wallSize / 2.
      let canvas = Canvas.makeCanvas canvasPixels canvasPixels

      let material = Material.standard |> Material.withColor (Color.create 0.5 0.1 0.7)
      let shape = Shape.sphere |> Shape.setMaterial material

      let lightPosition = Point.create -10. 10. -10.
      let lightColor = Color.create 1. 1. 1.
      let light = Light.create lightColor lightPosition

      let calc x y =
          let worldY = half - pixelSize * (float y)
          let worldX = -half + pixelSize * (float x)
          let position = Point.create worldX worldY (float wallZ)

          let ray =
              Vector.normalize (position - rayOrigin)
              |> Ray.create rayOrigin 

          let hit =
              ray
              |> Ray.intersect shape
              |> Intersection.hit

          

          match hit with
          | Some intersection ->
              let point = Ray.position intersection.t ray
              let normal = Shape.normal point intersection.object
              let eye = ray.direction * -1.
              let color = Material.lighting intersection.object.material light point eye normal
              Some {X = x; Y = y; Color = color}
          | _ -> None

      canvas
      |> Canvas.cordinats
      |> List.map (fun c -> calc c.X c.Y)
      |> List.choose (fun x -> x)
      |> List.iter(fun c -> Canvas.setPixel c.X c.Y c.Color canvas)

      let ppm = Canvas.toPPM canvas
      System.IO.File.WriteAllText ("image.pgm", ppm)
      *)
        (*
        image 5
        let m =
            Material.standard
            |> Material.withColor (Color.create 1. 0.9 0.9)
            |> Material.withSpecular 0.

        let floor =
            Shape.sphere
            |> Shape.transform (Scaling(10., 0.01, 10.))
            |> Shape.setMaterial m

        let leftWall =
            Shape.sphere
            |> Shape.transform (Translation(0., 0., 5.))
            |> Shape.transform (Rotation(Y, -(Math.PI/4.)))
            |> Shape.transform (Rotation(X, -(Math.PI/2.)))
            |> Shape.transform (Scaling(10., 0.01, 10.))
            |> Shape.setMaterial m

        let rightWall =
          Shape.sphere
          |> Shape.transform (Translation(0., 0., 5.))
          |> Shape.transform (Rotation(Y, (Math.PI/4.)))
          |> Shape.transform (Rotation(X, (Math.PI/2.)))
          |> Shape.transform (Scaling(10., 0.01, 10.))
          |> Shape.setMaterial m

        let middle =
            Shape.sphere
            |> Shape.transform (Translation(-0.5, 1., 0.5))
            |> Shape.setMaterial (
                Material.standard
                |> Material.withColor (Color.create 0.1 1. 0.5)
                |> Material.withDiffuse 0.7
                |> Material.withSpecular 0.3)

        let right =
            Shape.sphere
            |> Shape.transform (Translation(1.5, 0.5, 0.5))
            |> Shape.transform (Scaling(0.5, 0.5, 0.5))
            |> Shape.setMaterial (
                Material.standard
                |> Material.withColor (Color.create 0.5 1. 0.1)
                |> Material.withDiffuse 0.7
                |> Material.withSpecular 0.3)

        let left =
            Shape.sphere
            |> Shape.transform (Translation(-1.5, 0.33, -0.75))
            |> Shape.transform (Scaling(0.33, 0.33, 0.33))
            |> Shape.setMaterial (
                Material.standard
                |> Material.withColor (Color.create 1. 0.8 0.1)
                |> Material.withDiffuse 0.7
                |> Material.withSpecular 0.3)

        let world =
            World.empty
            |> World.addObject floor
            |> World.addObject leftWall
            |> World.addObject rightWall
            |> World.addObject middle
            |> World.addObject left
            |> World.addObject right
            |> World.withLight (
                Light.create (Color.create 1. 1. 1.) (Point.create -10. 10. -10.))

        let stopWatch = System.Diagnostics.Stopwatch.StartNew()
        let ppm =
            Camera.create 50 25 (Math.PI/3.)
            |> Camera.withTransfom (Transformation.viewTransform (Point.create 0. 1.5 -5.) (Point.create 0. 1. 0.) (Vector.create 0. 1. 0.))
            |> Camera.render world
            |> Canvas.toPPM
        stopWatch.Stop()

        System.IO.File.WriteAllText ("image.pgm", ppm)
        *)


    
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
        Camera.create 500 250 (Math.PI/3.)
        |> Camera.withTransfom (Transformation.viewTransform (Point.create 0. 1.5 -5.) (Point.create 0. 1. 0.) (Vector.create 0. 1. 0.))
        |> Camera.render world
        |> Canvas.toPPM
    stopWatch.Stop()

    System.IO.File.WriteAllText ("image.pgm", ppm)

    0    