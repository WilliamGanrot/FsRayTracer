module OBJFileTests
open RayTracer.OBJFile
open Xunit
open RayTracer.Triangle
open RayTracer.ObjectDomain
open RayTracer.RayDomain
open RayTracer.Point
open RayTracer.Group
open RayTracer.Helpers
open RayTracer.RayDomain
open RayTracer.Vector

[<Fact>]
let ``vertex records`` () =
    let s =
           "v -1 1 0\n\
            v -1.00 0.500 0.00\n\
            v 1 0 0\n\
            v 1 1 0"
    let parser = OBJFile.parseFile (s.Split "\n")

    let v = parser.vertices
    let v1 = v.[0]
    let v2 = v.[1]
    let v3 = v.[2]
    let v4 = v.[3]
    Point.equal (Point.create -1. 1. 0.) v1 |> Assert.True
    Point.equal (Point.create -1. 0.5 0.) v2|> Assert.True
    Point.equal (Point.create 1. 0. 0.) v3 |> Assert.True
    Point.equal (Point.create 1. 1. 0.) v4 |> Assert.True

[<Fact>]
let ``parsing triangle face`` () =
    let s =
           "v -1 1 0\n\
            v -1 0 0\n\
            v 1 0 0\n\
            v 1 1 0\n\
            f 1 2 3\n\
            f 1 3 4"

        
    let parser = OBJFile.parseFile (s.Split "\n")
    let t1 = parser.defaultGroup.[0]
    let t2 = parser.defaultGroup.[1]

    match t1.shape with
    | Triangle(p1,p2,p3,_,_,_) ->
        Point.equal p1 parser.vertices.[0] |> Assert.True
        Point.equal p2 parser.vertices.[1] |> Assert.True
        Point.equal p3 parser.vertices.[2] |> Assert.True
    | _ -> failwith "wrong shape"

    match t2.shape with
    | Triangle(p1,p2,p3,_,_,_) ->
        Point.equal p1 parser.vertices.[0] |> Assert.True
        Point.equal p2 parser.vertices.[2] |> Assert.True
        Point.equal p3 parser.vertices.[3] |> Assert.True
    | _ -> failwith "wrong shape"

[<Fact>]
let ``triangulating polygons`` () =
    let s =
           "v -1 1 0\n\
            v -1 0 0\n\
            v 1 0 0\n\
            v 1 1 0\n\
            v 0 2 0\n\
            f 1 2 3 4 5"

        
    let parser = OBJFile.parseFile (s.Split "\n")
    let t1 = parser.defaultGroup.[0]
    let t2 = parser.defaultGroup.[1]
    let t3 = parser.defaultGroup.[2]

    match t1.shape with
    | Triangle(p1,p2,p3,_,_,_) ->
        Point.equal p1 parser.vertices.[1-1] |> Assert.True
        Point.equal p2 parser.vertices.[2-1] |> Assert.True
        Point.equal p3 parser.vertices.[3-1] |> Assert.True
    | _ -> failwith "wrong shape"

    match t2.shape with
    | Triangle(p1,p2,p3,_,_,_) ->
        Point.equal p1 parser.vertices.[1-1] |> Assert.True
        Point.equal p2 parser.vertices.[3-1] |> Assert.True
        Point.equal p3 parser.vertices.[4-1] |> Assert.True
    | _ -> failwith "wrong shape"

    match t3.shape with
    | Triangle(p1,p2,p3,_,_,_) ->
        Point.equal p1 parser.vertices.[1-1] |> Assert.True
        Point.equal p2 parser.vertices.[4-1] |> Assert.True
        Point.equal p3 parser.vertices.[5-1] |> Assert.True
    | _ -> failwith "wrong shape"


[<Fact>]
let ``triangles in groups`` () =
    let s =
           "v -1 1 0\n\
            v -1 0 0\n\
            v 1 0 0\n\
            v 1 1 0\n\
            g FirstGroup\n\
            f 1 2 3\n\
            g SecoundGroup\n\
            f 1 3 4"

    let parser = OBJFile.parseFile (s.Split "\n")
    let g1 = parser.groups.[0]
    let g2 = parser.groups.[1]

    let t1 = g1 |> Group.getChildren |> List.head
    let t2 = g2 |> Group.getChildren |> List.head

    match t1.shape with
    | Triangle(p1,p2,p3,_,_,_) ->
        Point.equal p1 parser.vertices.[1-1] |> Assert.True
        Point.equal p2 parser.vertices.[2-1] |> Assert.True
        Point.equal p3 parser.vertices.[3-1] |> Assert.True
    | _ -> true |> Assert.False

    match t2.shape with
    | Triangle(p1,p2,p3,_,_,_) ->
        Point.equal p1 parser.vertices.[1-1] |> Assert.True
        Point.equal p2 parser.vertices.[3-1] |> Assert.True
        Point.equal p3 parser.vertices.[4-1] |> Assert.True
    | _ -> true |> Assert.False


[<Fact>]
let ``converting an obj file to a group`` () =
    let file =
           "v -1 1 0\n\
            v -1 0 0\n\
            v 1 0 0\n\
            v 1 1 0\n\
            g FirstGroup\n\
            f 1 2 3\n\
            g SecoundGroup\n\
            f 1 3 4"

    let parser = OBJFile.parseFile (file.Split "\n")
    parser.groups.Length = 2 |> Assert.True


[<Fact>]
let ``vertex normal records`` () =
    let file =
           "vn 0 0 1\n\
            vn 0.707 0 -0.707\n\
            vn 1 2 3"

    let parser = OBJFile.parseFile (file.Split "\n")
    parser.normals.[0] = (Vector.create 0. 0. 1.) |> Assert.True
    parser.normals.[1] = (Vector.create 0.707 0. -0.707) |> Assert.True
    parser.normals.[2] = (Vector.create 1. 2. 3.) |> Assert.True
   

[<Fact>]
let ``faces with normals`` () =
    let file =
           "v 0 1 0\n\
            v -1 0 0\n\
            v 1 0 0\n\
            vn -1 0 0\n\
            vn 1 0 0\n\
            vn 0 1 0\n\
            f 1//3 2//1 3//2\n\
            f 1/0/3 2/102/1 3/14/2"

    let parser = OBJFile.parseFile (file.Split "\n")
    let g = parser.defaultGroup
    let t1 = g.[0]
    let t2 = g.[1]

    match t1.shape with
    | SmoothTraingle(p1,p2,p3,_,_,n1,n2,n3) ->
        Point.equal p1 parser.vertices.[1-1] |> Assert.True
        Point.equal p2 parser.vertices.[2-1] |> Assert.True
        Point.equal p3 parser.vertices.[3-1] |> Assert.True

        n1 .= parser.normals.[3-1] |> Assert.True
        n2 .= parser.normals.[1-1] |> Assert.True
        n3 .= parser.normals.[2-1] |> Assert.True
    | _ -> failwith ""
    t2 .=. t1 |> Assert.True
   