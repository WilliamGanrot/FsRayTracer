module ComputationTests

open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Ray
open RayTracer.Matrix
open RayTracer.Intersection
open RayTracer.Helpers
open RayTracer.Computation
open RayTracer.Transformation
open RayTracer.World
open RayTracer.ObjectDomain

open Xunit
open RayTracer.Object
open RayTracer.Sphere
open RayTracer.Plane

open RayTracer.Constnats
open RayTracer.Material
open RayTracer.Color

[<Fact>]
let ``precomputing the state of an intersection`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.) 
    let s = Sphere.create()
    let i = Intersection.create s 4.
    let comps = i |> Computation.prepare r [i] []

    FloatHelper.equal comps.t i.t |> Assert.True
    comps.object .=. i.object |> Assert.True
    Point.create 0. 0. -1. |> Point.equal comps.point |> Assert.True
    comps.eyev .= Vector.create 0. 0. -1. |> Assert.True
    comps.normalv .= Vector.create 0. 0. -1. |> Assert.True

[<Fact>]
let ``the hit, when an intersection occurs on the outside`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.) 
    let s = Sphere.create()

    let comps =
        let i = Intersection.create s 4.
        Computation.prepare r [i] [] i

    comps.inside |> Assert.False

[<Fact>]
let ``the hit, when an intersection occurs on the inside`` () =
    let r = Ray.create (Point.create 0. 0. 0.) (Vector.create 0. 0. 1.) 
    let s = Sphere.create()
    let i = Intersection.create s 1.
    let comps = i |> Computation.prepare r [i] []

    comps.point |> Point.equal (Point.create 0. 0. 1.) |> Assert.True
    comps.eyev .= (Vector.create 0. 0. -1.) |> Assert.True
    comps.normalv .= Vector.create 0. 0. -1. |> Assert.True
    comps.inside |> Assert.True

[<Fact>]
let ``the hit should offset the point`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.) 
    let s =
        Sphere.create()
        |> Object.transform (Translation(0., 0., 0.1))

    let i = Intersection.create s 1.
    let comps = i |> Computation.prepare r [i] []

    comps.overPoint.Z < -(epsilon/2.) |> Assert.True
    comps.point.Z > comps.overPoint.Z |> Assert.True

[<Fact>]
let ``the Shilick approxiation under total internal relfection`` () =
    let r = Ray.create (Point.create 0. 0. (Math.Sqrt(2.)/2.)) (Vector.create 0. 1. 0.)
    let shape = Sphere.createGlass()

    let xs = Intersection.tuplesToIntersections [((-Math.Sqrt(2.)/2.), shape); ((Math.Sqrt(2.)/2.), shape)]

    let comps = Computation.prepare r xs [] xs.[1]
    let reflectance = Computation.shlick comps
    FloatHelper.equal reflectance 1. |> Assert.True

[<Fact>]
let ``the schlick approximation with a perpendicular viewing angle`` () =
    let r = Ray.create (Point.create 0. 0. 0.) (Vector.create 0. 1. 0.)
    let shape = Sphere.createGlass()

    let xs = Intersection.tuplesToIntersections [(-1., shape); (1., shape)]

    let comps = Computation.prepare r xs [] xs.[1]
    let reflectance = Computation.shlick comps

    FloatHelper.equal reflectance 0.04 |> Assert.True

[<Fact>]
let ``the schlick approxiamtion with small anle and n2 > n1`` () =
    let r = Ray.create (Point.create 0. 0.99 -2.) (Vector.create 0. 0. 1.)
    let shape = Sphere.createGlass()

    let xs = Intersection.tuplesToIntersections [(1.8589, shape);]

    let comps = Computation.prepare r xs [] xs.[0]
    let reflectance = Computation.shlick comps

    FloatHelper.equal reflectance 0.48873 |> Assert.True


[<Fact>]
let ``shadehit with a reflective transparent material`` () =

    let floor =
        Plane.create()
        |> Object.transform (Translation(0., -1., 0.))
        |> Object.setMaterial (
            Material.standard
            |> Material.withReflectivity 0.5
            |> Material.withTransparency 0.5
            |> Material.WithrefractiveIndex 1.5)
    let ball =
        Sphere.create()
        |> Object.setMaterial (
            Material.standard
            |> Material.withColor (Color.create 1. 0. 0.)
            |> Material.withAmbient 0.5)
        |> Object.transform (Translation(0., -3.5, -0.5))

    let w =
        World.standard
        |> World.addObject floor
        |> World.addObject ball

    let r = Ray.create (Point.create 0. 0. -3.) (Vector.create 0. (-Math.Sqrt(2.)/2.) (Math.Sqrt(2.)/2.))

    let xs = Intersection.tuplesToIntersections [(Math.Sqrt(2.), floor)]
    let comps = Computation.prepare r xs [] xs.[0]

    let color = World.shadeHit w 5 comps

    color .= Color.create 0.93391 0.69643 0.69243 |> Assert.True


