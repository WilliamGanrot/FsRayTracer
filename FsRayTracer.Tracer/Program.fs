open RayTracer.Vector
open RayTracer.Point
open RayTracer.Color
open System
open Raytracer.Canvas
open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Shape
open RayTracer.Intersection
open RayTracer.Ray

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

    0    