namespace RayTracer.Sphere
open RayTracer.RayDomain
open RayTracer.Intersection
open RayTracer.ObjectDomain
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Helpers
open RayTracer.Material
open RayTracer.Transformation
open RayTracer.Object
open RayTracer.Matrix
open System

module Sphere =

    let localIntersect object ray =

        let sphereToRay = ray.origin - Point.create 0. 0. 0.
        let a = Vector.dot ray.direction ray.direction
        let b = 2. * (Vector.dot ray.direction sphereToRay)
        let c = (Vector.dot sphereToRay sphereToRay) - 1.
        let discriminated = (b*b) - (4. * a * c)

        match discriminated with
        | d when d < 0. -> []
        | _ ->
            let t1 = ((-b) - Math.Pow(discriminated, 0.5)) / (2. * a)
            let t2 = ((-b) + Math.Pow(discriminated, 0.5)) / (2. * a)

            let i1 = Intersection.create object t1
            let i2 = Intersection.create object t2
            [i1;i2]

    let localNormalAt shape objectPoint =
        objectPoint - (Point.create 0. 0. 0.)

    let create() =

        { transform = Matrix.identityMatrix 4;
          transformInverse = Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = Sphere;
          id = newRandom();
          localIntersect = localIntersect;
          localNormalAt = localNormalAt;
          parent = None }

    let createGlass() =

        let m =
            Material.standard
            |> Material.toGlass

        { transform = Matrix.identityMatrix 4;
          transformInverse = Matrix.identityMatrix 4 |> Matrix.inverse;
          material = m;
          shape = Sphere;
          id = newRandom();
          localIntersect = localIntersect;
          localNormalAt = localNormalAt;
          parent = None }
