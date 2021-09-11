module MaterialTests

open System
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Helpers
open RayTracer.Light
open RayTracer.Material
open RayTracer.Color
open RayTracer.Intersection
open RayTracer.World
open RayTracer.Matrix
open RayTracer.Transformation
open RayTracer.Computation
open RayTracer.Object
open RayTracer.ObjectDomain

open Xunit
open RayTracer.Ray

[<Fact>]
let ``the default material``  () =
    let m = Material.standard

    m.ambient |> FloatHelper.equal 0.1 |> Assert.True
    m.diffuse |> FloatHelper.equal 0.9 |> Assert.True
    m.specular |> FloatHelper.equal 0.9 |> Assert.True
    m.shininess |> FloatHelper.equal 200. |> Assert.True


[<Fact>]
let ``Lightning with eye between the light and surface``() =
    let m = Material.standard
    let p = Point.create 0. 0. 0.
    let eyev = Vector.create 0. 0. -1.
    let normalv = Vector.create 0. 0. -1.
    let light = Light.create (Color.create 1. 1. 1.) (Point.create 0. 0. -10.)

    let result = Object.lighting m light p eyev normalv false (Object.sphere())
    result .= (Color.create 1.9 1.9 1.9) |> Assert.True


[<Fact>]
let ``Lightning with surface in shadow``() =
    let m = Material.standard
    let p = Point.create 0. 0. 0.
    let eyev = Vector.create 0. 0. -1.
    let normalv = Vector.create 0. 0. -1.
    let light = Light.create (Color.create 1. 1. 1.) (Point.create 0. 0. -10.)
    let inShadow = true

    let result = Object.lighting m light p eyev normalv inShadow (Object.sphere())
    result .= (Color.create 0.1 0.1 0.1) |> Assert.True

[<Fact>]
let ``Lightning with eye eye between liht and surface eye offset 40 degrees``() =
    let m = Material.standard
    let p = Point.create 0. 0. 0.
    let eyev = Vector.create 0. (Math.Pow(2., 0.5)/2.) -(Math.Pow(2., 0.5)/2.)
    let normalv = Vector.create 0. 0. -1.
    let light = Light.create (Color.create 1. 1. 1.) (Point.create 0. 0. -10.)

    let result = Object.lighting m light p eyev normalv false (Object.sphere())
    result .= (Color.create 1. 1. 1.) |> Assert.True
    
[<Fact>]
let ``Lightning with eye opposite surface light offset 45 degrees``() =
        let m = Material.standard
        let p = Point.create 0. 0. 0.
        let eyev = Vector.create 0. 0. -1.
        let normalv = Vector.create 0. 0. -1.
        let light = Light.create (Color.create 1. 1. 1.) (Point.create 0. 10. -10.)

        let result = Object.lighting m light p eyev normalv false (Object.sphere())
        result .= (Color.create 0.7364 0.7364 0.7364) |> Assert.True
    
[<Fact>]
let ``Lightning with eye in the path of the feflection vector``() =
    let m = Material.standard
    let p = Point.create 0. 0. 0.
    let eyev = Vector.create 0. -(Math.Pow(2., 0.5)/2.) -(Math.Pow(2., 0.5)/2.)
    let normalv = Vector.create 0. 0. -1.
    let light = Light.create (Color.create 1. 1. 1.) (Point.create 0. 10. -10.)

    let result = Object.lighting m light p eyev normalv false (Object.sphere())
    result .= (Color.create 1.6364 1.6364 1.6364) |> Assert.True

[<Fact>]
let ``Lightning with eye behind the surface``() =
    let m = Material.standard
    let p = Point.create 0. 0. 0.
    let eyev = Vector.create 0. 0. -1.
    let normalv = Vector.create 0. 0. -1.
    let light = Light.create (Color.create 1. 1. 1.) (Point.create 0. 0. 10.)

    let result = Object.lighting m light p eyev normalv false (Object.sphere())
    result .= (Color.create 0.1 0.1 0.1) |> Assert.True

[<Fact>]
let ``Reflectivity for the default material``() =
    let m = Material.standard
    m.reflectivity = 0. |> Assert.True


[<Fact>]
let ``precomputing the reflection vector``() =
    let s = Object.plane()
    let r = Ray.create (Point.create 0. 1. -1.) (Vector.create 0. ((-(Math.Sqrt(2.)))/2.) ((Math.Sqrt(2.))/2.))
    let i = Intersection.create s (Math.Sqrt(2.))
    
    let comps = Computation.prepare r [i] i
    comps.reflectv .= Vector.create 0. (Math.Sqrt(2.)/2.) ((Math.Sqrt(2.)/2.)) |> Assert.True


[<Fact>]
let ``transparency and reflextion index for the default materialr``() =
    let m = Material.standard
    m.transparency = 0. |> Assert.True
    m.refractiveIndex = 1. |> Assert.True

[<Fact>]
let ``a helper for producing a sphere with a glassy material``() =
    let s = Object.glassSphere()
    s.transform .= Matrix.identityMatrix 4 |> Assert.True
    s.material.transparency = 1. |> Assert.True
    s.material.refractiveIndex = 1.5 |> Assert.True


[<Fact>]
let ``finding n1 and n2 at varios intersections``() =
    let a =
        Object.glassSphere()
        |> Object.transform (Scaling(2., 2., 2.))
        |> Object.setMaterial(
            Material.standard
            |> Material.WithrefractiveIndex 1.5)

    let b =
        Object.glassSphere()
        |> Object.transform (Translation(0., 0., -0.25))
        |> Object.setMaterial(
            Material.standard
            |> Material.WithrefractiveIndex 2.)

    let c =
        Object.glassSphere()
        |> Object.transform (Translation(0., 0., 0.25))
        |> Object.setMaterial(
            Material.standard
            |> Material.WithrefractiveIndex 2.5)

    let ray = Ray.create (Point.create 0. 0. -4.) (Vector.create 0. 0. 1.)

    let xs = Intersection.tuplesToIntersections [(2.,a); (2.75,b); (3.25, c); (4.75,b); (5.25,c); (6.,a)]

    let expected =
        [ (1., 1.5);
          (1.5, 2.);
          (2., 2.5);
          (2.5, 2.5);
          (2.5, 1.5);
          (1.5, 1.)]

    xs
    |> List.indexed
    |> List.iter (fun (i,o) ->
        let comps = Computation.nValues o ray xs
        let e = expected.[i]
        comps = e |> Assert.True
        )

