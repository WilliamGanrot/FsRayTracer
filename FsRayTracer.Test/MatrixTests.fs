module MatrixTests

open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Constnats
open RayTracer.Matrix
open RayTracer.Helpers



open Xunit

[<Fact>]
let ``constructing and inspecting a 4*4 matrix`` () =
    let m = 
        Matrix.make [ [1.;      2.;     3.;     4.];
                      [5.5;     6.5;    7.5;    8.5;];
                      [9.;      10.;    11.;    12.];
                      [13.5;    14.5;   15.5;   16.5] ]

    FloatHelper.equal 1. m.entries.[0,0] |> Assert.True
    FloatHelper.equal 4. m.entries.[0,3] |> Assert.True
    FloatHelper.equal 5.5 m.entries.[1,0] |> Assert.True
    FloatHelper.equal 7.5 m.entries.[1,2] |> Assert.True
    FloatHelper.equal 11. m.entries.[2,2] |> Assert.True
    FloatHelper.equal 13.5 m.entries.[3,0] |> Assert.True
    FloatHelper.equal 15.5 m.entries.[3,2] |> Assert.True

[<Fact>]
let ``a 2*2 matric ought to be representable`` () =
    let m = 
        Matrix.make [ [-3.; 5.];
                      [1.;  -2.] ]

    FloatHelper.equal -3. m.entries.[0,0] |> Assert.True
    FloatHelper.equal 5. m.entries.[0,1] |> Assert.True
    FloatHelper.equal 1. m.entries.[1,0] |> Assert.True
    FloatHelper.equal -2. m.entries.[1,1] |> Assert.True

[<Fact>]
let ``a 3*3 matric ought to be representable`` () =
    let m = 
        Matrix.make [ [-3.;     5.;     0.];
                      [1.;      -2.;    -7.];
                      [0.;      1.;    1.] ]

    FloatHelper.equal -3. m.entries.[0,0] |> Assert.True
    FloatHelper.equal -2. m.entries.[1,1] |> Assert.True
    FloatHelper.equal 1. m.entries.[2,2] |> Assert.True

[<Fact>]
let ``matric equality with identical matrices`` () =
    let m1 = 
        Matrix.make [ [1.;      2.;     3.;     4.];
                      [5.;      6.6;    7.;     8.];
                      [9.;      8.;     7.;     6.];
                      [5.;      4.;     3.;     2.] ]

    let m2 = 
        Matrix.make [ [1.;      2.;     3.;     4.];
                      [5.;      6.6;    7.;     8.];
                      [9.;      8.;     7.;     6.];
                      [5.;      4.;     3.;     2.] ]

    m1 = m2 |> Assert.True

[<Fact>]
let ``matric equality with diffrent matices`` () =
    let a = 
        Matrix.make [ [1.;      2.;     3.;     4.];
                      [5.;      6.6;    7.;     8.];
                      [9.;      8.;     7.;     6.];
                      [5.;      4.;     3.;     2.] ]

    let b = 
        Matrix.make [ [2.;      3.;     4.;     5.];
                      [6.;      7.;     8.;     9.];
                      [8.;      7.;     6.;     5.];
                      [4.;      3.;     2.;     1.] ]

    a <> b |> Assert.True

[<Fact>]
let ``multiply two matrices`` () =
    let a = 
        Matrix.make [ [1.;      2.;     3.;     4.];
                      [5.;      6.;    7.;     8.];
                      [9.;      8.;     7.;     6.];
                      [5.;      4.;     3.;     2.] ]

    let b = 
        Matrix.make [ [-2.;      1.;     2.;     3.];
                      [3.;      2.;     1.;     -1.];
                      [4.;      3.;     6.;     5.];
                      [1.;      2.;     7.;     8.] ]

    let expected = 
        Matrix.make [ [20.;      22.;     50.;     48.];
                      [44.;      54.;     114.;     108.];
                      [40.;      58.;     110.;     102.];
                      [16.;      26.;     46.;     42.] ]


    let newM = a * b 
    newM = expected |> Assert.True


[<Fact>]
let ``a mtrix multiplied by a point`` () =
    let a = 
        Matrix.make [ [1.;      2.;     3.;     4.];
                      [2.;      4.;    4.;     2.];
                      [8.;      6.;     4.;     1.];
                      [0.;      0.;     0.;     1.] ]

    let expected = Point.create 18. 24. 33. 
    let multiplied = Matrix.multiplyPoint a (Point.create 1. 2. 3.)
    expected = multiplied |> Assert.True



[<Fact>]
let ``a mtrix multiplied by a vector`` () =
    let a = 
        Matrix.make [ [1.;      2.;     3.;     4.];
                      [2.;      4.;    4.;     2.];
                      [8.;      6.;     4.;     1.];
                      [0.;      0.;     0.;     1.] ]

    let expected = Vector.create 14. 22. 32. 
    let multiplied = Matrix.multiplyVector a (Vector.create 1. 2. 3.)
    expected = multiplied |> Assert.True



[<Fact>]
let ``multiplying a matrix by the identity matrix`` () =
    let a = 
        Matrix.make [ [1.;      2.;     3.;     4.];
                      [2.;      4.;    4.;     2.];
                      [8.;      6.;     4.;     1.];
                      [0.;      0.;     0.;     1.] ]

    let identity = Matrix.identityMatrix a.dimensions
    a * identity = a |> Assert.True


[<Fact>]
let ``multiplying the identity with matric by a vector`` () =

    let v = Vector.create 4. 4. 4.
    let identity = Matrix.identityMatrix 4

    let multiplied = v |> Matrix.multiplyVector identity
    multiplied .= v |> Assert.True

[<Fact>]
let ``transposing a matrix`` () =

    let a = 
        Matrix.make [ [0.;      9.;     3.;     0.];
                      [9.;      8.;    0.;     8.];
                      [1.;      8.;     5.;     3.];
                      [0.;      0.;     5.;     8.] ]

    let expected = 
        Matrix.make [ [0.;      9.;     1.;     0.];
                      [9.;      8.;    8.;     0.];
                      [3.;      0.;     5.;     5.];
                      [0.;      8.;     3.;     8.] ]

    let x = a |> Matrix.Transpose
    x = expected |> Assert.True


[<Fact>]
let ``transposing the identity matrix`` () =
    let identity = Matrix.identityMatrix 4
    let n = Matrix.Transpose identity
    n = identity |> Assert.True


[<Fact>]
let ``calculating the determinant of a 2*2 m`` () =
    let m =
        Matrix.make[ [1.;   5.]
                     [-3.;  2.]]

    let d = Matrix.determinant m
    17. = d |> Assert.True



[<Fact>]
let ``a submatrix of a 3*3 matrix is a 2*2 matrix`` () =
    let a =
        Matrix.make[ [1.;   5.; 0.]
                     [-3.;  2.; 7.]
                     [0.;   6.; -3.]]
    let expected =
        Matrix.make[ [-3.;   2.]
                     [0.;  6.]]

    expected = Matrix.submatrix 0 2 a |> Assert.True

[<Fact>]
let ``a submatrix of a 4*4 matrix is a 3*3 matrix`` () =
    let a =
        Matrix.make[ [-6.;      1.;     1.;     6.]
                     [-8.;      5.;     8.;     6.]
                     [-1.;      0.;     8.;     2.]
                     [-7.;      1.;     -1.;    1.]]
    let expected =
        Matrix.make[ [-6.;      1.;     6.]
                     [-8.;      8.;     6.]
                     [-7.;      -1.;    1.] ]

    expected = Matrix.submatrix 2 1 a |> Assert.True

[<Fact>]
let ``calculating a minor of a 3*3 matrix`` () =
    let a =
        Matrix.make[ [3.;      5.;     0.]
                     [2.;      -1.;     -7.]
                     [6.;      -1.;     5.]]

    let b = Matrix.submatrix 1 0 a

    25. = Matrix.determinant b |> Assert.True
    25. = Matrix.minor 1 0 a |> Assert.True

[<Fact>]
let ``calculating a cofactor of a 3*3 matrix`` () =
    let a =
        Matrix.make[ [3.;      5.;     0.]
                     [2.;      -1.;     -7.]
                     [6.;      -1.;     5.]]

    FloatHelper.equal -12. (Matrix.minor 0 0 a) |> Assert.True
    FloatHelper.equal -12. (Matrix.cofactor 0 0 a) |> Assert.True

    FloatHelper.equal 25. (Matrix.minor 1 0 a) |> Assert.True
    FloatHelper.equal -25. (Matrix.cofactor 1 0 a) |> Assert.True

[<Fact>]
let ``calculating the determinant of a 3*3 matrix`` () =
    let a =
        Matrix.make[ [1.;      2.;     6.]
                     [-5.;      8.;     -4.]
                     [2.;      6.;     4.]]

    FloatHelper.equal 56. (Matrix.cofactor 0 0 a) |> Assert.True
    FloatHelper.equal 12. (Matrix.cofactor 0 1 a) |> Assert.True
    FloatHelper.equal -46. (Matrix.cofactor 0 2 a) |> Assert.True
    FloatHelper.equal -196. (Matrix.determinant a) |> Assert.True


[<Fact>]
let ``calculating the determinant of a 4*4 matrix`` () =

    let a =
        Matrix.make[ [-2.;      -8.;     3.;     5.]
                     [-3.;      1.;     7.;     3.]
                     [1.;      2.;     -9.;     6.]
                     [-6.;      7.;     7.;    -9.]]

    FloatHelper.equal 690. (Matrix.cofactor 0 0 a) |> Assert.True
    FloatHelper.equal 447. (Matrix.cofactor 0 1 a) |> Assert.True
    FloatHelper.equal 210. (Matrix.cofactor 0 2 a) |> Assert.True
    FloatHelper.equal 51. (Matrix.cofactor 0 3 a) |> Assert.True
    FloatHelper.equal -4071. (Matrix.determinant a) |> Assert.True
    

[<Fact>]
let ``testing an invertible matrix for inveribility`` () =

    let a =
        Matrix.make[ [6.;      4.;     4.;     4.]
                     [5.;      5.;     7.;     6.]
                     [4.;      -9.;     3.;     -7.]
                     [9.;      1.;     7.;    -6.]]

    
    FloatHelper.equal -2120. (Matrix.determinant a) |> Assert.True
    Matrix.invertable a |> Assert.True    

[<Fact>]
let ``testing a noninvertible matrix for inveribiltiy`` () =

    let a =
        Matrix.make[ [-4.;      2.;     -2.;     -3.]
                     [9.;      6.;     2.;     6.]
                     [0.;      -5.;     1.;     -5.]
                     [0.;      0.;     0.;    0.]]

    FloatHelper.equal 0. (Matrix.determinant a) |> Assert.True
    Matrix.invertable a |> Assert.False

[<Fact>]
let ``calculating the inverse of a matrix`` () =

    let a =
        Matrix.make [ [ -5.0; 2.; 6.; -8.0 ];
                      [ 1.; -5.0; 1.; 8. ];
                      [ 7.; 7.0; -6.0; -7.0 ];
                      [ 1.; -3.0; 7.; 4. ] ]


    let b = a |> Matrix.inverse

    let expected =
        Matrix.make [ [ 0.21805; 0.45113; 0.24060; -0.04511 ];
                       [ -0.80827; -1.45677; -0.44361; 0.52068 ];
                       [ -0.07895; -0.22368; -0.05263; 0.19737 ];
                       [ -0.52256; -0.81391; -0.30075; 0.30639 ] ]


    FloatHelper.equal 532. (Matrix.determinant a) |> Assert.True
    FloatHelper.equal -160. (Matrix.cofactor 2 3 a) |> Assert.True
    FloatHelper.equal (-160./532.) b.entries.[3,2]  |> Assert.True
    FloatHelper.equal 105. (Matrix.cofactor 3 2 a) |> Assert.True
    FloatHelper.equal (105./532.) b.entries.[2,3] |> Assert.True

    b .= expected |> Assert.True

[<Fact>]
let ``calculating the inverse of a another matrix`` () =

    let a =
        Matrix.make [ [ 8.; -5.0; 9.; 2. ]
                      [ 7.; 5.; 6.; 1. ]
                      [ -6.0; 0.; 9.; 6. ]
                      [ -3.0; 0.; -9.0; -4.0 ] ]
    
    let expected =
        Matrix.make [ [ -0.15385; -0.15385; -0.28205; -0.53846 ];
                      [ -0.07692; 0.12308; 0.02564; 0.03077 ];
                      [ 0.35897; 0.35897; 0.43590; 0.92308 ];
                      [ -0.69231; -0.69231; -0.76923; -1.92308 ] ]

    let b = a |> Matrix.inverse

    b .= expected |> Assert.True

[<Fact>]
let ``calculating the inverse of a third matrix`` () =

    let a =
        Matrix.make [ [ 9.; 3.; 0.; 9. ];
                        [ -5.0; -2.0; -6.0; -3.0 ];
                        [ -4.0; 9.; 6.; 4. ];
                        [ -7.0; 6.; 6.; 2. ] ]

    
    let expected =
        Matrix.make[[ -0.04074; -0.07778; 0.14444; -0.22222 ];
                    [ -0.07778; 0.03333; 0.36667; -0.33333 ];
                    [ -0.02901; -0.1463; -0.10926; 0.12963 ];
                    [ 0.17778; 0.06667; -0.26667; 0.33333 ] ]


    let b = a |> Matrix.inverse

    b .= expected |> Assert.True

[<Fact>]
let ``multiplying a product by its inverse`` () =

    let a =
        Matrix.make [ [ 3.;     -9.;    7.;     3.];
                      [ 3.;     -8.;    2.;   -9. ];
                      [ -4.;   4.;     4.;     1. ];
                      [ -6.;   5.;     -1.;     1. ] ]

    let b =
        Matrix.make [ [ 8.;     2.;    2.;    2.];
                      [ 3.;     -1.;    7.;   0. ];
                      [ 7.;   0.;     5.;     4. ];
                      [ 6.;   -2.;     0.;     5. ] ]

    let c = a * b
    a .= c * Matrix.inverse b |> Assert.True