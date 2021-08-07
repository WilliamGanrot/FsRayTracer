
module VectorTests

open System
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Helpers

open Xunit

[<Fact>]
let ``Can create vector`` () =
    let made = Vector.create 4.3 1.0 1.1
    Assert.Equal(made.W, 0.0)

[<Fact>]
let ``Can compare vectors`` () =
    let made = Vector.create 4.3 1.0 1.1
    let made2 = Vector.create 4.3 1.0 1.10000001

    let isEqual = made .= made2
    Assert.True(isEqual)

[<Fact>]
let ``Can add vectors`` () =
    let made = Vector.create 4.3 1.0 1.1
    let made2 = Vector.create 2.2 -1.0 1.1

    let sum = made + made2
    Assert.Equal(sum.X, made.X + made2.X)
    Assert.Equal(sum.Y, made.Y + made2.Y)
    Assert.Equal(sum.Z, made.Z + made2.Z)

[<Fact>]
let ``Subtracting two vectors`` () =
    let v1 = Vector.create 3.0 2.0 1.0
    let v2 = Vector.create 5.0 6.0 7.0

    let result = v1 - v2
    let expected = Vector.create -2.0 -4.0 -6.0

    Assert.Equal(result, expected)

[<Fact>]
let ``Negating a vector`` () =
    let vector = Vector.create 1.0 -2.0 3.0
    let expected = Vector.create -1.0 2.0 -3.0

    Assert.True(vector.Negate .= expected)

[<Fact>]
let ``multiply vector with scalar`` () =
    let vector = Vector.create 1.0 -2.0 3.0

    let multiplied = vector |> Vector.multiblyByScalar
    let expected = Vector.create 3.5 -7.0 10.5

    let equals = multiplied .= expected

    Assert.True(equals)

[<Fact>]
let ``computing magnitud of vector 1,0,0`` () =
    let v = Vector.create 1.0 0.0 0.0
    let m = v |> Vector.magnitude

    FloatHelper.equal m 1. |> Assert.True

[<Fact>]
let ``computing magnitud of vector 0,1,0`` () =
    let v = Vector.create 0.0 1.0 0.0
    let m = v |> Vector.magnitude

    FloatHelper.equal m 1. |> Assert.True

[<Fact>]

let ``computing magnitud of vector 0,0,1`` () =
    let v = Vector.create 0.0 0.0 1.0
    let m = v |> Vector.magnitude

    FloatHelper.equal m 1. |> Assert.True

[<Fact>]
let ``computing magnitud of vector 1,2,3`` () =
    let v = Vector.create 1.0 2.0 3.0
    let m = v |> Vector.magnitude

    Assert.True(FloatHelper.equal (14.0**0.5) m)

[<Fact>]
let ``computing magnitud of vector -1,-2,-3`` () =
    let v = Vector.create -1.0 -2.0 -3.0
    let m = v |> Vector.magnitude

    FloatHelper.equal (14.0**0.5) m |> Assert.True


[<Fact>]
let ``normalising vector 4,0,0`` () =
    let n = Vector.create 4.0 0.0 0.0 |> Vector.normalize
    let equals = n .= Vector.create 1.0 0.0 0.0

    Assert.True(equals)

[<Fact>]
let ``normalising vector 1,2,3`` () =
    let n = Vector.create 1.0 2.0 3.0 |> Vector.normalize
    let equals = n .= Vector.create 0.26726 0.53452 0.80178

    Assert.True(equals)

[<Fact>]
let ``the magnitude of normalized vector`` () =

    let m =
        Vector.create 1.0 2.0 3.0
        |> Vector.normalize
        |> Vector.magnitude

    FloatHelper.equal m 1. |> Assert.True

[<Fact>]
let ``the cross product of two vectors`` () =

    let a = Vector.create 1.0 2.0 3.0
    let b = Vector.create 2.0 3.0 4.0

    Assert.True((Vector.cross a b) .= Vector.create -1.0 2.0 -1.0)
    Assert.True((Vector.cross b a) .= Vector.create 1.0 -2.0 1.0)

[<Fact>]
let ``reflecting a vector approaching at 45``  () =

    let v = Vector.create 1. -1. 0.
    let n = Vector.create 0. 1. 0.

    let r = Vector.reflect n v

    r .= (Vector.create 1. 1. 0.) |> Assert.True

[<Fact>]
let ``reflecting a vector of slanted surface``  () =

    let v = Vector.create 0. -1. 0.
    let n = Vector.create (Math.Pow(2., 0.5)/2.) (Math.Pow(2., 0.5)/2.) 0.

    let r = Vector.reflect n v

    r .= (Vector.create 1. 0. 0.) |> Assert.True