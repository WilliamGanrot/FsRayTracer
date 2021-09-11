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
open RayTracer.Domain
open RayTracer.ObjectDomain
open RayTracer.Sphere

open Xunit
open RayTracer.Object


[<Fact>]
let ``a spheres deafult transformation`` () =
    let s = Object.sphere()
    (s.transform) .= (Matrix.identityMatrix 4) |> Assert.True

[<Fact>]
let ``changing a phere 's transformation`` () =
    let s = Sphere.create()
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

[<Theory>]
[<InlineData(5., 0.5, 0., -1., 0., 0., 4., 6.)>]
[<InlineData(-5., 0.5, 0., 1., 0., 0., 4., 6.)>]
[<InlineData(0.5, 5., 0., 0., -1., 0., 4., 6.)>]
[<InlineData(0.5, -5., 0., 0., 1., 0., 4., 6.)>]
[<InlineData(0.5, 0., 5., 0., 0., -1., 4., 6.)>]
[<InlineData(0.5, 0., -5., 0., 0., 1., 4., 6.)>]
[<InlineData(0., 0.5, 0., 0., 0., 1., -1., 1.)>]
let ``A ray intersects a cube`` (pointX, pointY, pointZ, vectorX, vectorY, vecotrZ, t1, t2) =
    let c = Object.cube()
    let r = Ray.create (Point.create pointX pointY pointZ) (Vector.create vectorX vectorY vecotrZ)
    let xs = Ray.intersect c r

    xs.Length = 2 |> Assert.True
    FloatHelper.equal xs.[0].t t1 |> Assert.True
    FloatHelper.equal xs.[1].t t2 |> Assert.True

[<Theory>]
[<InlineData(-2., 0., 0., 0.2673, 0.5345, 0.8018)>]
[<InlineData(0., -2., 0., 0.8018, 0.2673, 0.5345)>]
[<InlineData(0., 0., -2., 0.5345, 0.8018, 0.2673)>]
[<InlineData(2., 0., -2., 0., 0., -1.)>]
[<InlineData(0., 2., 2., 0., -1., 0.)>]
[<InlineData(2., 2., 0., -1., 0., 0.)>]

let ``A ray misses a cube`` (pointX, pointY, pointZ, vectorX, vectorY, vecotrZ) =
    let c = Cube.create
    let r = Ray.create (Point.create pointX pointY pointZ) (Vector.create vectorX vectorY vecotrZ)
    let xs = Ray.intersect c r

    xs.Length = 0 |> Assert.True

[<Theory>]
[<InlineData(1., 0.5, -0.8, 1., 0., 0.)>]
[<InlineData(-1., -0.2, 0.9, -1., 0., 0.)>]
[<InlineData(-0.4, 1., -0.1, 0., 1., 0.)>]
[<InlineData(0.3, -1., -0.7, 0., -1., 0.)>]
[<InlineData(-0.6, 0.3, 1., 0., 0., 1.)>]
[<InlineData(0.4 ,0.4, -1., 0., 0., -1.)>]
[<InlineData(1., 1., 1., 1., 0., 0.)>]
[<InlineData(-1., -1., -1., -1., 0., 0.)>]

let ``the normal surface of a cube`` (pointX, pointY, pointZ, vectorX, vectorY, vecotrZ) =

    let c = Object.cube()
    let p = Point.create pointX pointY pointZ

    let normal = Object.normal p c
    let expected = Vector.create vectorX vectorY vecotrZ

    expected .= normal |> Assert.True


[<Theory>]
[<InlineData(1., 0., 0., 0., 1., 0.)>]
[<InlineData(1., 0., 0., 0., 1., 0.)>]
[<InlineData(0., 0., -5., 1., 1., 1.)>]
let ``a ray misses a cylinder`` (pointX, pointY, pointZ, vectorX, vectorY, vecotorZ) =
    
    let c = Object.cylinder()
    let directon = Vector.create vectorX vectorY vecotorZ |> Vector.normalize

    let r = Ray.create (Point.create pointX pointY pointZ) directon 

    let xs = Ray.intersect c r
    xs.Length = 0 |> Assert.True
    
[<Theory>]
[<InlineData(1., 0., -5., 0., 0., 1., 5., 5.)>]
[<InlineData(0., 0., -5., 0., 0., 1., 4., 6.)>]
[<InlineData(0.5, 0., -5., 0.1, 1., 1., 6.80798, 7.08872)>]
let ``a ray strikes a cylinder`` (pointX, pointY, pointZ, vectorX, vectorY, vecotorZ, t0, t1) =
    
    let cyl = Object.cylinder()
    let directon = Vector.create vectorX vectorY vecotorZ |> Vector.normalize
    let r = Ray.create (Point.create pointX pointY pointZ) directon 

    let xs = Ray.intersect cyl r

    xs.Length = 2 |> Assert.True
    FloatHelper.equal xs.[0].t t0 |> Assert.True
    FloatHelper.equal xs.[1].t t1 |> Assert.True
        
[<Theory>]
[<InlineData(1., 0., 0., 1., 0., 0.)>]
[<InlineData(0., 5., -1., 0., 0., -1.)>]
[<InlineData(0., -2., 1., 0., 0., 1.)>]
[<InlineData(-1., 1., 0., -1., 0., 0.)>]
let ``normal vector on a cylinder`` (pointX, pointY, pointZ, vectorX, vectorY, vecotorZ) =
    
    let cyl = Object.cylinder()
    let p = Point.create pointX pointY pointZ
    let n = Object.normal p cyl

    n .= (Vector.create vectorX vectorY vecotorZ) |> Assert.True

[<Fact>]
let ``the default min and max for a cylinder`` () =
    let cyl = Object.cylinder()
    match cyl.shape with
    | Cylinder (min, max, closed) ->
        min = -infinity |> Assert.True
        max = infinity |> Assert.True


[<Theory>]
[<InlineData(0., 1.5, 0., 0.1, 1., 0., 0)>]
[<InlineData(0., 3., -5., 0., 0., 1., 0)>]
[<InlineData(0., 0., -5., 0., 0., 1., 0)>]
[<InlineData(0., 2., -5., 0., 0., 1., 0)>]
[<InlineData(0., 1., -5., 0., 0., 1., 0)>]
[<InlineData(0., 1.5, -2., 0., 0., 1., 2)>]
let ``the intersecting a constrained cylinder`` (pointX, pointY, pointZ, vectorX, vectorY, vecotorZ, count) =

    let cyl = {Object.cylinder() with shape = Cylinder(1., 2., false)}
    let direction = Vector.create vectorX vectorY vecotorZ |> Vector.normalize
    let r = Ray.create (Point.create pointX pointY pointZ) direction

    let xs = Ray.intersect cyl r
    xs.Length = count |> Assert.True


[<Fact>]
let ``the default closed vlye for a cylinder`` () =
    let cyl = Object.cylinder()
    match cyl.shape with
    | Cylinder (min, max, closed) ->
        min = -infinity |> Assert.True
        max = infinity |> Assert.True


[<Theory>]
[<InlineData(0., 3., 0., 0., -1., 0., 2)>]
[<InlineData(0., 3., -2., 0., -1., 2., 2)>]
[<InlineData(0., 4., -2., 0., -1., 1., 2)>]
[<InlineData(0., 0., -2., 0., 1., 2., 2)>]
[<InlineData(0., -1., -2., 0., 1., 1., 2)>]
let ``intersecting the caps of a closed cylinder`` (pointX, pointY, pointZ, vectorX, vectorY, vectorZ, count) =

    let cyl = { Object.cylinder() with shape = Cylinder(1., 2., true) }
    let direction = Vector.create vectorX vectorY vectorZ |> Vector.normalize
    let r = Ray.create (Point.create pointX pointY pointZ) direction

    let xs = Ray.intersect cyl r
    Assert.Equal(count, xs.Length)


[<Theory>]
[<InlineData(0., 1., 0., 0., -1., 0.)>]
[<InlineData(0.5, 1., 0., 0., -1., 0.)>]
[<InlineData(0., 1., 0.5, 0., -1., 0.)>]
[<InlineData(0., 2., 0., 0., 1., 0.)>]
[<InlineData(0.5, 2., 0., 0., 1., 0.)>]
[<InlineData(0., 2., 0.5, 0., 1., 0.)>]
let ``the normal vector on a cylinder's end caps`` (pointX, pointY, pointZ, vectorX, vectorY, vecotorZ) =

    let cyl = {Object.cylinder() with shape = Cylinder(1., 2., true)}
    let n = Object.normal (Point.create pointX pointY pointZ) cyl
    let normal = Vector.create vectorX vectorY vecotorZ

    n .= normal |> Assert.True


[<Theory>]
[<InlineData(0., 0., -5., 0., 0., 1., 5., 5.)>]
[<InlineData(0., 0., -5., 1., 1., 1., 8.66025, 8.66025)>]
[<InlineData(1., 1., -5., -0.5, -1., 1., 4.55006, 49.44994)>]
let ``intersecting a cone with a ray`` (pointX, pointY, pointZ, vectorX, vectorY, vecotorZ, t0, t1) =

    let cone = Object.cone()
    let direction = Vector.create vectorX vectorY vecotorZ |> Vector.normalize
    let r = Ray.create (Point.create pointX pointY pointZ) direction

    let xs = Ray.intersect cone r
    xs.Length = 2 |> Assert.True
    FloatHelper.equal xs.[0].t t0 |> Assert.True
    FloatHelper.equal xs.[1].t t1 |> Assert.True

[<Fact>]
let ``intersecting a cone with a ray parallel to one of its halves`` () =
    let cyl = Object.cone()
    let direction = Vector.create 0. 1. 1. |> Vector.normalize
    let r = Ray.create (Point.create 0. 0. -1.) direction
    let xs = Ray.intersect cyl r

    xs.Length = 1 |> Assert.True
    FloatHelper.equal xs.[0].t 0.35355 |> Assert.True


[<Theory>]
[<InlineData(0., 0., -5., 0., 1., 0., 0)>]
[<InlineData(0., 0., -0.25, 0., 1., 1., 2)>]
[<InlineData(0., 0., -0.25, 0., 1., 0., 4)>]
let ``intersecting a cone's end caps`` (pointX, pointY, pointZ, vectorX, vectorY, vecotorZ, count) =

    let c = { Object.cone() with shape = Cone(-0.5, 0.5, true) }
    let direction = Vector.create vectorX vectorY vecotorZ |> Vector.normalize
    let r = Ray.create (Point.create pointX pointY pointZ) direction
    let xs = Ray.intersect c r
    xs.Length = count |> Assert.True

[<Theory>]
[<InlineData(0., 0., 0., 0., 0., 0.)>]
[<InlineData(1., 1., 1., 1., -1.41421356237, 1.)>]
[<InlineData(-1., -1., 0., -1., 1., 0.)>]
let ``computing the normal vector on a cone`` (pointX, pointY, pointZ, vectorX, vectorY, vectorZ) =

    let cyl = Object.cone()

    let n = Object.normal (Point.create pointX pointY pointZ) cyl
    n .= (Vector.create vectorX vectorY vectorZ) |> Assert.True

