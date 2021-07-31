
module VectorTests

open System
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Helpers

open Xunit

[<Fact>]
let ``Can create vector`` () =
    let made = createVector (4.3, 1.0, 1.1)
    Assert.Equal(made.W, 0.0)

[<Fact>]
let ``Can compare vectors`` () =
    let made = createVector (4.3, 1.0, 1.1)
    let made2 = createVector (4.3, 1.0, 1.10000001)

    let isEqual = made .= made2
    Assert.True(isEqual)

[<Fact>]
let ``Can add vectors`` () =
    let made = createVector (4.3, 1.0, 1.1)
    let made2 = createVector (2.2, -1.0, 1.1)

    let sum = made + made2
    Assert.Equal(sum.X, made.X + made2.X)
    Assert.Equal(sum.Y, made.Y + made2.Y)
    Assert.Equal(sum.Z, made.Z + made2.Z)

[<Fact>]
let ``Subtracting two vectors`` () =
    let v1 = createVector (3.0,2.0,1.0)
    let v2 = createVector (5.0,6.0,7.0)

    let result = v1 - v2
    let expected = createVector(-2.0,-4.0,-6.0)

    Assert.Equal(result, expected)

[<Fact>]
let ``Negating a vector`` () =
    let vector = createVector (1.0, -2.0, 3.0)
    let expected = createVector (-1.0, 2.0, -3.0)

    Assert.True(vector.Negate .= expected)

[<Fact>]
let ``multiply vector with scalar`` () =
    let vector = createVector (1.0, -2.0, 3.0)

    let multiplied = vector |> multiblyByScalar
    let expected = createVector(3.5, -7.0, 10.5)

    let equals = multiplied .= expected

    Assert.True(equals)

[<Fact>]
let ``computing magnitud of vector 1,0,0`` () =
    let v = createVector(1.0,0.0,0.0)
    let m = v |> magnitude

    FloatHelper.equal m 1. |> Assert.True

[<Fact>]
let ``computing magnitud of vector 0,1,0`` () =
    let v = createVector(0.0,1.0,0.0)
    let m = v |> magnitude

    FloatHelper.equal m 1. |> Assert.True

[<Fact>]

let ``computing magnitud of vector 0,0,1`` () =
    let v = createVector(0.0,0.0,1.0)
    let m = v |> magnitude

    FloatHelper.equal m 1. |> Assert.True

[<Fact>]
let ``computing magnitud of vector 1,2,3`` () =
    let v = createVector(1.0,2.0,3.0)
    let m = v |> magnitude

    Assert.True(FloatHelper.equal (14.0**0.5) m)

[<Fact>]
let ``computing magnitud of vector -1,-2,-3`` () =
    let v = createVector(-1.0,-2.0,-3.0)
    let m = v |> magnitude

    FloatHelper.equal (14.0**0.5) m |> Assert.True


[<Fact>]
let ``normalising vector 4,0,0`` () =
    let n = createVector(4.0, 0.0, 0.0) |> normalize
    let equals = n .= createVector(1.0, 0.0, 0.0)

    Assert.True(equals)

[<Fact>]
let ``normalising vector 1,2,3`` () =
    let n = createVector(1.0, 2.0, 3.0) |> normalize
    let equals = n .= createVector(0.26726, 0.53452, 0.80178)

    Assert.True(equals)

[<Fact>]
let ``the magnitude of normalized vector`` () =

    let m =
        createVector(1.0, 2.0, 3.0)
        |> normalize
        |> magnitude

    FloatHelper.equal m 1. |> Assert.True

[<Fact>]
let ``the cross product of two vectors`` () =

    let a = createVector(1.0,2.0,3.0)
    let b = createVector(2.0,3.0,4.0)

    Assert.True((cross a b) .= createVector(-1.0,2.0,-1.0))
    Assert.True((cross b a) .= createVector(1.0,-2.0,1.0))