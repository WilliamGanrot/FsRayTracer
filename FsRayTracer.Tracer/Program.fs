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
open RayTracer.Object
open RayTracer.World
open RayTracer.Camera
open RayTracer.Pattern

type Projectile = {
    position:Point;
    velocity:Vector;
}

type Enviorment = {
    gravity:Vector;
    wind:Vector;
}

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


        (* image 6
        
        let m =
            Material.standard
            |> Material.withColor (Color.create 1. 0.9 0.9)
            |> Material.withSpecular 0.

        let floor =
            Object.sphere()
            |> Object.transform (Scaling(10., 0.01, 10.))
            |> Object.setMaterial m

        let leftWall =
            Object.sphere()
            |> Object.transform (Translation(0., 0., 5.))
            |> Object.transform (Rotation(Y, -(Math.PI/4.)))
            |> Object.transform (Rotation(X, -(Math.PI/2.)))
            |> Object.transform (Scaling(10., 0.01, 10.))
            |> Object.setMaterial m

        let rightWall =
          Object.sphere()
          |> Object.transform (Translation(0., 0., 5.))
          |> Object.transform (Rotation(Y, (Math.PI/4.)))
          |> Object.transform (Rotation(X, (Math.PI/2.)))
          |> Object.transform (Scaling(10., 0.01, 10.))
          |> Object.setMaterial m

        let middle =
            Object.sphere()
            |> Object.transform (Translation(-0.5, 1., 0.5))
            |> Object.setMaterial (
                Material.standard
                |> Material.withColor (Color.create 0.1 1. 0.5)
                |> Material.withDiffuse 0.7
                |> Material.withSpecular 0.3)

        let right =
            Object.sphere()
            |> Object.transform (Translation(1.5, 0.5, 0.5))
            |> Object.transform (Scaling(0.5, 0.5, 0.5))
            |> Object.setMaterial (
                Material.standard
                |> Material.withColor (Color.create 0.5 1. 0.1)
                |> Material.withDiffuse 0.7
                |> Material.withSpecular 0.3)

        let left =
            Object.sphere()
            |> Object.transform (Translation(-1.5, 0.33, -0.75))
            |> Object.transform (Scaling(0.33, 0.33, 0.33))
            |> Object.setMaterial (
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

    
    //let p =
    //    Object.plane()
    //    |> Object.setMaterial (
    //        Material.standard
    //        |> Material.withPattern (
    //            Pattern.rings (Color.white) (Color.create 0.7 0. 0.2)
    //            |> Pattern.transform (Translation(0., 0., 2.5))))
    
    //let middle =
    //    Object.sphere()
    //    |> Object.setMaterial (
    //        Material.standard
    //        |> Material.withPattern (
    //            Pattern.stripes (Color.create 0.3 0. 0.) (Color.create 0.5 1. 0.7)
    //            |> Pattern.transform (Scaling(0.2, 0.2, 0.2))
    //            |> Pattern.transform (Rotation(Y, -Math.PI/4.)))
    //        |> Material.withDiffuse 0.7
    //        |> Material.withSpecular 0.3)
    //    |> Object.transform (Translation(-0.5, 1., 0.5))



    //let right =
    //    Object.sphere()
    //    |> Object.transform (Translation(1.5, 0.5, 0.5))
    //    |> Object.transform (Scaling(0.5, 0.5, 0.5))
    //    |> Object.setMaterial (
    //        Material.standard
    //        |> Material.withColor (Color.create 0.9 0.7 0.)
    //        |> Material.withDiffuse 0.7
    //        |> Material.withSpecular 0.3)



    //let left =
    //    Object.sphere()
    //    |> Object.transform (Translation(-1.5, 0.25, -0.75))
    //    |> Object.transform (Scaling(0.75, 0.75, 0.75))
    //    |> Object.setMaterial (
    //        Material.standard
    //        |> Material.withPattern (
    //            Pattern.gradient (Color.create 0. 0.1 0.6) (Color.create 0. 0.9 0.5)
    //            |> Pattern.transform (Scaling(2., 1., 1.))
    //            |> Pattern.transform (Translation(-0.5, 0., 0.)))
    //        |> Material.withDiffuse 0.7
    //        |> Material.withSpecular 0.3)


    //let world =
    //    World.empty
    //    |> World.addObject p
    //    |> World.addObject middle
    //    |> World.addObject left
    //    |> World.addObject right
    //    |> World.withLight (
    //        Light.create (Color.create 1. 1. 1.) (Point.create -10. 10. -10.))

    //let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    //let ppm =
    //    Camera.create 1200 600 (Math.PI/3.)
    //    |> Camera.withTransfom (Transformation.viewTransform (Point.create 0. 1.5 -5.) (Point.create 0. 1. 0.) (Vector.create 0. 1. 0.))
    //    |> Camera.render world
    //    |> Canvas.toPPM
    //stopWatch.Stop()

    //System.IO.File.WriteAllText ("image.pgm", ppm)


    
    let p =
        Object.plane()
        |> Object.setMaterial (
            Material.standard
            |> Material.withPattern (
                Pattern.checkers (Color.white) (Color.black)
                |> Pattern.transform (Rotation(Y, 45.)))
            |> Material.withReflectivity 0.5)

    let middle =
        Object.sphere()
        |> Object.setMaterial (
            Material.standard
            |> Material.withDiffuse 0.7
            |> Material.withSpecular 1.
            |> Material.withShininess 300.
            |> Material.withReflectivity 0.3
            |> Material.withColor (Color.create 0.7 0. 0.2))
        |> Object.transform (Translation(0.75, 1., 1.5))

    let glass =
        Object.glassSphere()
        |> Object.transform (Translation(-0.75, 0.75, 0.))
        |> Object.transform (Scaling(0.75, 0.75, 0.75))
        |> Object.setMaterial(
            Material.standard
            |> Material.withColor (Color.create 0. 0. 0.)
            |> Material.withDiffuse 0.
            |> Material.withSpecular 0.8
            |> Material.withReflectivity 1.
            |> Material.withTransparency 1.
            |> Material.WithrefractiveIndex 1.5
            )



    let world =
        World.empty
        |> World.addObject p
        |> World.addObject middle
        |> World.addObject glass
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