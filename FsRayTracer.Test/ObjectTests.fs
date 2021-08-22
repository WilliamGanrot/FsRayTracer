module ShapeTests

open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Ray
open RayTracer.Matrix
open RayTracer.Intersection
open RayTracer.Helpers
open RayTracer.Transformation
open RayTracer.Material
open RayTracer.Light
open RayTracer.Color

open Xunit
open RayTracer.Object


[<Fact>]
let ``a spheres deafult transformation`` () =
    let s = Object.sphere()
    (s.transform) .= (Matrix.identityMatrix 4) |> Assert.True

[<Fact>]
let ``changing a phere 's transformation`` () =
    let s = Object.sphere()
    let t = Translation(2., 3., 4.)

    let s2 = Object.transform t s

    s2.transform .= (Transformation.matrix t) |> Assert.True

[<Fact>]
let ``intersecting a scaled sphere with a ray`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)
    let s = Object.sphere()

    let s2 = s |> Object.transform (Scaling(2., 2., 2.))
    let xs = Ray.intersect s2 r

    xs.Length = 2 |> Assert.True
    (FloatHelper.equal xs.[0].t 3.) |> Assert.True
    (FloatHelper.equal xs.[1].t 7.) |> Assert.True

[<Fact>]
let ``intersecting a translated sphere with a ray`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)
    let s = Object.sphere()

    let s2 = s |> Object.transform (Translation(5., 0., 0.))
    let xs = Ray.intersect s2 r

    xs.Length = 0 |> Assert.True

[<Fact>]
let ``the normal on a sphere poin on the x axis`` () =
    let p = Point.create 1. 0. 0.
    let s = Object.sphere()
    let n = s |> Object.normal p

    n .= (Vector.create 1. 0. 0.) |> Assert.True

[<Fact>]
let ``the normal on a sphere poin on the y axis`` () =
    let p = Point.create 0. 1. 0.
    let s = Object.sphere()
    let n = s |> Object.normal p

    n .= (Vector.create 0. 1. 0.) |> Assert.True

[<Fact>]
let ``the normal on a sphere poin on the z axis`` () =
    let p = Point.create 0. 0. 1.
    let s = Object.sphere()
    let n = s |> Object.normal p

    n .= (Vector.create 0. 0. 1.) |> Assert.True

[<Fact>]
let ``the normal is a normalized vector`` () =
    let p = Point.create (Math.Pow (3., 0.5)/3.) (Math.Pow (3., 0.5)/3.) (Math.Pow (3., 0.5)/3.)
    let s = Object.sphere()
    let n = s |> Object.normal p

    n .= Vector.normalize (n) |> Assert.True

[<Fact>]
let ``cumputing the normal on a translated sphere`` () =
    let n =
        Object.sphere()
        |> Object.transform (Translation(0., 1., 0.))
        |> Object.normal (Point.create 0. 1.70711 -0.70711)

    n .= (Vector.create 0. 0.70711 -0.70711) |>  Assert.True

[<Fact>]
let ``cumputing the normal on a transformed sphere`` () =
    let n =
        Object.sphere()
        |> Object.transform (Scaling(1., 0.5, 1.))
        |> Object.transform (Rotation(Z, (Math.PI/5.)))
        |> Object.normal (Point.create 0. (Math.Pow (2., 0.5)/2.) -(Math.Pow (2., 0.5)/2.))

    n .= (Vector.create 0. 0.97014 -0.24254) |>  Assert.True

[<Fact>]
let ``a sphere has a deafult material`` () =
    let s = Object.sphere()
    s.material = Material.standard |> Assert.True

[<Fact>]
let ``a sphere may be assigned a material`` () =
    
    let m =
        Material.standard
        |> Material.withAmbient 1.

    let s =
        Object.sphere()
        |> Object.setMaterial m

    s.material = m |> Assert.True


    