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
    (*IMAGE !*)
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

    let c = Canvas.makeCanvas 200 200
    let origin = Point.create 0. 0. 0.

    let angle = (2.*Math.PI)/12.
    let radius = 3./8. * float c.Width

    let twelwP = Point.create 0. 0. 1.

    let points =
        [1..12]
        |> List.map(fun h ->

            let t = Translation(100., 100., 100.)
            Rotation(Y, (float h) * angle)
            |> Transformation.matrix
            |> Matrix.multiplyPoint twelwP
            |> Matrix.multiplyPoint (t |> Transformation.matrix))
    points
    |> List.map (fun p -> c |> Canvas.setPixel (int (p.X*radius)+100) (int (p.Y*radius)+100) Color.red)
    |> ignore
    //let twelwP = Point.create 0. 0. 1.
    //let threeP =
    //    Rotation (Y, 3. * angle)
    //    |> Transformation.matrix
    //    |> Matrix.multiplyPoint twelwP

    //c |> Canvas.setPixel (int (twelwP.X*radius + origin.X)) (int (twelwP.Z*radius + origin.Z)) Color.red
    //c |> Canvas.setPixel (int (threeP.X*radius + origin.X)) (int (threeP.Z*radius + origin.Z)) Color.white
    let ppm = Canvas.toPPM c
    System.IO.File.WriteAllText("image.pgm", ppm);
    0    