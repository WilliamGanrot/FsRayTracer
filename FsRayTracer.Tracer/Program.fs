open RayTracer.Vector
open RayTracer.Point
open RayTracer.Color
open System
open Raytracer.Canvas

type Projectile = {
    position:Point;
    velocity:Vector;
}

type Enviorment = {
    gravity:Vector;
    wind:Vector;
}

let tick env proj =
    let pos = proj.position + proj.velocity
    let velocity = proj.velocity + env.gravity + env.wind
    {position = pos; velocity = velocity}


[<EntryPoint>]
let main argv =

    let tick environment projectile =
        { position = projectile.position + projectile.velocity;
          velocity = projectile.velocity + environment.gravity + environment.wind }

    let p =
        { position = createPoint(0.0, 1.0, 0.0);
          velocity = (createVector(1.0, 1.8, 0.0) |> normalize) * 11.25 }

    let e =
        { gravity = createVector(0.0, -0.1, 0.0);
          wind    = createVector(-0.01, 0.0, 0.0) }

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
    0    