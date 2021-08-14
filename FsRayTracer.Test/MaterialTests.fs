module MaterialTests

open System
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Helpers
open RayTracer.Light
open RayTracer.Material
open RayTracer.Color

open Xunit

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

    let result = Material.lighting m light p eyev normalv false
    result .= (Color.create 1.9 1.9 1.9) |> Assert.True

[<Fact>]
let ``Lightning with surface in shadow``() =
    let m = Material.standard
    let p = Point.create 0. 0. 0.
    let eyev = Vector.create 0. 0. -1.
    let normalv = Vector.create 0. 0. -1.
    let light = Light.create (Color.create 1. 1. 1.) (Point.create 0. 0. -10.)
    let inShadow = true

    let result = Material.lighting m light p eyev normalv inShadow
    result .= (Color.create 0.1 0.1 0.1) |> Assert.True

[<Fact>]
let ``Lightning with eye eye between liht and surface eye offset 40 degrees``() =
    let m = Material.standard
    let p = Point.create 0. 0. 0.
    let eyev = Vector.create 0. (Math.Pow(2., 0.5)/2.) -(Math.Pow(2., 0.5)/2.)
    let normalv = Vector.create 0. 0. -1.
    let light = Light.create (Color.create 1. 1. 1.) (Point.create 0. 0. -10.)

    let result = Material.lighting m light p eyev normalv false
    result .= (Color.create 1. 1. 1.) |> Assert.True
    
[<Fact>]
let ``Lightning with eye opposite surface light offset 45 degrees``() =
        let m = Material.standard
        let p = Point.create 0. 0. 0.
        let eyev = Vector.create 0. 0. -1.
        let normalv = Vector.create 0. 0. -1.
        let light = Light.create (Color.create 1. 1. 1.) (Point.create 0. 10. -10.)

        let result = Material.lighting m light p eyev normalv false
        result .= (Color.create 0.7364 0.7364 0.7364) |> Assert.True
    
[<Fact>]
let ``Lightning with eye in the path of the feflection vector``() =
    let m = Material.standard
    let p = Point.create 0. 0. 0.
    let eyev = Vector.create 0. -(Math.Pow(2., 0.5)/2.) -(Math.Pow(2., 0.5)/2.)
    let normalv = Vector.create 0. 0. -1.
    let light = Light.create (Color.create 1. 1. 1.) (Point.create 0. 10. -10.)

    let result = Material.lighting m light p eyev normalv false
    result .= (Color.create 1.6364 1.6364 1.6364) |> Assert.True

[<Fact>]
let ``Lightning with eye behind the surface``() =
    let m = Material.standard
    let p = Point.create 0. 0. 0.
    let eyev = Vector.create 0. 0. -1.
    let normalv = Vector.create 0. 0. -1.
    let light = Light.create (Color.create 1. 1. 1.) (Point.create 0. 0. 10.)

    let result = Material.lighting m light p eyev normalv false
    result .= (Color.create 0.1 0.1 0.1) |> Assert.True