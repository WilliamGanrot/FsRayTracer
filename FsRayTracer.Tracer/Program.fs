open RayTracer.Vector
open RayTracer.Point
open RayTracer.Color
open System
open Raytracer.Canvas
open RayTracer.Transformation
open RayTracer.Matrix

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
    0    