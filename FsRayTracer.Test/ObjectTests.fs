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
open RayTracer.ObjectDomain
open RayTracer.Sphere
open RayTracer.Cube
open RayTracer.Cylinder
open RayTracer.Group
open RayTracer.Cone


open Xunit
open RayTracer.Object


[<Fact>]
let ``a spheres deafult transformation`` () =
    let s = Sphere.create()
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
    let s = Sphere.create()

    let s2 = s |> Object.transform (Scaling(2., 2., 2.))
    let xs = Ray.intersect s2 r

    xs.Length = 2 |> Assert.True
    (FloatHelper.equal xs.[0].t 3.) |> Assert.True
    (FloatHelper.equal xs.[1].t 7.) |> Assert.True

[<Fact>]
let ``intersecting a translated sphere with a ray`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)
    let s = Sphere.create()

    let s2 = s |> Object.transform (Translation(5., 0., 0.))
    let xs = Ray.intersect s2 r

    xs.Length = 0 |> Assert.True

[<Fact>]
let ``the normal on a sphere poin on the x axis`` () =
    let p = Point.create 1. 0. 0.
    let s = Sphere.create()
    let n = s |> Object.normal p []

    n .= (Vector.create 1. 0. 0.) |> Assert.True

[<Fact>]
let ``the normal on a sphere poin on the y axis`` () =
    let p = Point.create 0. 1. 0.
    let s = Sphere.create()
    let n = s |> Object.normal p []

    n .= (Vector.create 0. 1. 0.) |> Assert.True

[<Fact>]
let ``the normal on a sphere poin on the z axis`` () =
    let p = Point.create 0. 0. 1.
    let s = Sphere.create()
    let n = s |> Object.normal p []

    n .= (Vector.create 0. 0. 1.) |> Assert.True

[<Fact>]
let ``the normal is a normalized vector`` () =
    let p = Point.create (Math.Pow (3., 0.5)/3.) (Math.Pow (3., 0.5)/3.) (Math.Pow (3., 0.5)/3.)
    let s = Sphere.create()
    let n = s |> Object.normal p []

    n .= Vector.normalize (n) |> Assert.True

[<Fact>]
let ``cumputing the normal on a translated sphere`` () =
    let n =
        Sphere.create()
        |> Object.transform (Translation(0., 1., 0.))
        

    let n' = Object.normal (Point.create 0. 1.70711 -0.70711) [n] n
    n' .= (Vector.create 0. 0.70711 -0.70711) |>  Assert.True

[<Fact>]
let ``cumputing the normal on a transformed sphere`` () =
    let n =
        Sphere.create()
        |> Object.transform (Scaling(1., 0.5, 1.))
        |> Object.transform (Rotation(Z, (Math.PI/5.)))
    let n' = Object.normal (Point.create 0. (Math.Pow (2., 0.5)/2.) -(Math.Pow (2., 0.5)/2.)) [n] n

    n' .= (Vector.create 0. 0.97014 -0.24254) |>  Assert.True

[<Fact>]
let ``a sphere has a deafult material`` () =
    let s = Sphere.create()
    s.material = Material.standard |> Assert.True

[<Fact>]
let ``a sphere may be assigned a material`` () =
    
    let m =
        Material.standard
        |> Material.withAmbient 1.

    let s =
        Sphere.create()
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
    let c = Cube.create()
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
    let c = Cube.create()
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

    let c = Cube.create()
    let p = Point.create pointX pointY pointZ

    let normal = Object.normal p [] c
    let expected = Vector.create vectorX vectorY vecotrZ

    expected .= normal |> Assert.True


[<Theory>]
[<InlineData(1., 0., 0., 0., 1., 0.)>]
[<InlineData(1., 0., 0., 0., 1., 0.)>]
[<InlineData(0., 0., -5., 1., 1., 1.)>]
let ``a ray misses a cylinder`` (pointX, pointY, pointZ, vectorX, vectorY, vecotorZ) =
    
    let c = Cylinder.create()
    let directon = Vector.create vectorX vectorY vecotorZ |> Vector.normalize

    let r = Ray.create (Point.create pointX pointY pointZ) directon 

    let xs = Ray.intersect c r
    xs.Length = 0 |> Assert.True
    
[<Theory>]
[<InlineData(1., 0., -5., 0., 0., 1., 5., 5.)>]
[<InlineData(0., 0., -5., 0., 0., 1., 4., 6.)>]
[<InlineData(0.5, 0., -5., 0.1, 1., 1., 6.80798, 7.08872)>]
let ``a ray strikes a cylinder`` (pointX, pointY, pointZ, vectorX, vectorY, vecotorZ, t0, t1) =
    
    let cyl = Cylinder.create()
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
    
    let cyl = Cylinder.create()
    let p = Point.create pointX pointY pointZ
    let n = Object.normal p [] cyl

    n .= (Vector.create vectorX vectorY vecotorZ) |> Assert.True

[<Fact>]
let ``the default min and max for a cylinder`` () =
    let cyl = Cylinder.create()
    match cyl.shape with
    | Cylinder (min, max, closed) ->
        min = -infinity |> Assert.True
        max = infinity |> Assert.True
    | _ -> true |> Assert.False


[<Theory>]
[<InlineData(0., 1.5, 0., 0.1, 1., 0., 0)>]
[<InlineData(0., 3., -5., 0., 0., 1., 0)>]
[<InlineData(0., 0., -5., 0., 0., 1., 0)>]
[<InlineData(0., 2., -5., 0., 0., 1., 0)>]
[<InlineData(0., 1., -5., 0., 0., 1., 0)>]
[<InlineData(0., 1.5, -2., 0., 0., 1., 2)>]
let ``the intersecting a constrained cylinder`` (pointX, pointY, pointZ, vectorX, vectorY, vecotorZ, count) =

    let cyl = {Cylinder.create() with shape = Cylinder(1., 2., false)}
    let direction = Vector.create vectorX vectorY vecotorZ |> Vector.normalize
    let r = Ray.create (Point.create pointX pointY pointZ) direction

    let xs = Ray.intersect cyl r
    xs.Length = count |> Assert.True


[<Fact>]
let ``the default closed vlye for a cylinder`` () =
    let cyl = Cylinder.create()
    match cyl.shape with
    | Cylinder (min, max, closed) ->
        min = -infinity |> Assert.True
        max = infinity |> Assert.True
    | _ -> true |> Assert.False


[<Theory>]
[<InlineData(0., 3., 0., 0., -1., 0., 2)>]
[<InlineData(0., 3., -2., 0., -1., 2., 2)>]
[<InlineData(0., 4., -2., 0., -1., 1., 2)>]
[<InlineData(0., 0., -2., 0., 1., 2., 2)>]
[<InlineData(0., -1., -2., 0., 1., 1., 2)>]
let ``intersecting the caps of a closed cylinder`` (pointX, pointY, pointZ, vectorX, vectorY, vectorZ, count) =

    let cyl = { Cylinder.create() with shape = Cylinder(1., 2., true) }
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

    let cyl = {Cylinder.create() with shape = Cylinder(1., 2., true)}
    let n = Object.normal (Point.create pointX pointY pointZ) [] cyl
    let normal = Vector.create vectorX vectorY vecotorZ

    n .= normal |> Assert.True


[<Theory>]
[<InlineData(0., 0., -5., 0., 0., 1., 5., 5.)>]
[<InlineData(0., 0., -5., 1., 1., 1., 8.66025, 8.66025)>]
[<InlineData(1., 1., -5., -0.5, -1., 1., 4.55006, 49.44994)>]
let ``intersecting a cone with a ray`` (pointX, pointY, pointZ, vectorX, vectorY, vecotorZ, t0, t1) =

    let cone = Cone.create()
    let direction = Vector.create vectorX vectorY vecotorZ |> Vector.normalize
    let r = Ray.create (Point.create pointX pointY pointZ) direction

    let xs = Ray.intersect cone r
    xs.Length = 2 |> Assert.True
    FloatHelper.equal xs.[0].t t0 |> Assert.True
    FloatHelper.equal xs.[1].t t1 |> Assert.True

[<Fact>]
let ``intersecting a cone with a ray parallel to one of its halves`` () =
    let cyl = Cone.create()
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

    let c = { Cone.create() with shape = Cone(-0.5, 0.5, true) }
    let direction = Vector.create vectorX vectorY vecotorZ |> Vector.normalize
    let r = Ray.create (Point.create pointX pointY pointZ) direction
    let xs = Ray.intersect c r
    xs.Length = count |> Assert.True

[<Theory>]
[<InlineData(0., 0., 0., 0., 0., 0.)>]
[<InlineData(1., 1., 1., 1., -1.41421356237, 1.)>]
[<InlineData(-1., -1., 0., -1., 1., 0.)>]
let ``computing the normal vector on a cone`` (pointX, pointY, pointZ, vectorX, vectorY, vectorZ) =

    let cyl = Cone.create()
    let point = Point.create pointX pointY pointZ
    let normal = Vector.create vectorX vectorY vectorZ

    let n = cyl.localNormalAt cyl.shape point

    n .= (Vector.create vectorX vectorY vectorZ) |> Assert.True


[<Fact>]
let ``creating a new group`` () =
    let g = Group.create()

    g.transform .= Matrix.identityMatrix 4 |> Assert.True
    match g.shape with
    | Group l -> l.Length = 0 |> Assert.True
    | _ -> false |> Assert.True 

[<Fact>]
let ``intersecing a ray with an empty group`` () =
    let g = Group.create()
    let r = Ray.create (Point.create 0. 0. 0.) (Vector.create 0. 0. 1.)
    let xs = Ray.intersect g r
    xs.IsEmpty |> Assert.True

[<Fact>]
let ``intersecing a ray with a non empty group`` () =

    let s1 = Sphere.create()
    let s2 = Sphere.create() |> Object.transform (Translation(0., 0., -3.))
    let s3 = Sphere.create() |> Object.transform (Translation(5., 0., 0.))

    let g = 
        Group.create()
        |> Group.add s1 |> (fun (_, group) -> group )
        |> Group.add s2 |> (fun (_,group) -> group )
        |> Group.add s3 |> (fun (_,group) -> group )



    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)
    let xs = Ray.intersect g r
    xs.Length = 4 |> Assert.True
    xs.[0].object .=. s2 |> Assert.True
    xs.[1].object .=. s2 |> Assert.True
    xs.[2].object .=. s1 |> Assert.True
    xs.[3].object .=. s1 |> Assert.True


[<Fact>]
let ``intersecting a transformad group`` () =
    let g = Group.create() |> Object.transform (Scaling(2., 2., 2.))

    let s = Sphere.create() |> Object.transform (Translation(5., 0., 0.))

    let (_,g') = Group.add s g

    let r = Ray.create (Point.create 10. 0. -10.) (Vector.create 0. 0. 1.)

    let xs = Ray.intersect g' r
    xs.Length = 2 |> Assert.True

[<Fact>]
let ``convering a point from world to object space`` () =

    let s = Sphere.create() |> Object.transform (Translation(5., 0., 0.))
    let g1 = Group.create() |> Object.transform (Rotation(Y, Math.PI/2.))
    let g2 =  Group.create() |> Object.transform (Scaling(2., 2., 2.))

    let g1' = Group.setChildren [s] g1
    let g2' = Group.setChildren [g1'] g2

    //let (g2', g1') = Group.add g2 g1
    //let (child, g2'') = Group.add s g2'
    //let l = [g2'']

    let p = Object.worldToObject s (Point.create -2. 0. -10.) [g2']
    let expected = Point.create 0. 0. -1.
    Point.equal p expected |> Assert.True

[<Fact>]
let ``convering a normal from object to world space`` () =

    let g1 = Group.create() |> Object.transform (Rotation(Y,Math.PI/2.))
    let g2 =  Group.create() |> Object.transform (Scaling(1., 2., 3.))
    let s = Sphere.create() |> Object.transform (Translation(5., 0., 0.))

    let g2' = Group.setChildren [s] g2
    let g1' = Group.setChildren [g2'] g1

    let sqr = (Math.Sqrt(3.)/3.)
    let n = Object.normalToWorld s (Vector.create sqr sqr sqr) [g1']
    n .= Vector.create 0.2857 0.4286 -0.8571 |> Assert.True

[<Fact>]
let ``finding the normal onn a child object`` () =

    let g1 = Group.create() |> Object.transform (Rotation(Y,Math.PI/2.))
    let g2 =  Group.create() |> Object.transform (Scaling(1., 2., 3.))
    let s = Sphere.create() |> Object.transform (Translation(5., 0., 0.))

    let g2' = Group.setChildren [s] g2
    let g1' = Group.setChildren [g2'] g1
    let x = g1'
    

    let n = Object.normal (Point.create 1.7321 1.1547 -5.5774) [g1'] s
    n .= Vector.create 0.2857 0.4286 -0.8571 |> Assert.True
