namespace RayTracer.OBJFile
open RayTracer.Sphere
open RayTracer.ObjectDomain
open FSharp.Text.RegexProvider
open RayTracer.RayDomain
open RayTracer.Ray
open RayTracer.Point
open System.Collections.Generic
open RayTracer.Triangle
open RayTracer.Vector
open RayTracer.Group

module OBJFile =

    type ParseResult = { defaultGroup: Object list; vertices: Point list; groups: Object list; topGroup: Object; normals: Vector list}
    type LexResult = { shapes: Object list; vertices: Point list; normals: Vector list}

    type VertexRegexPattern = Regex< @"(?<shape>[v]) (?<f1>(-?[0-9]\d*(\.\d+)?)) (?<f2>(-?[0-9]\d*(\.\d+)?)) (?<f3>(-?[0-9]\d*(\.\d+)?))$" >
    type NormalRegexPattern = Regex< @"(?<shape>[vn]) (?<f1>(-?[0-9]\d*(\.\d+)?)) (?<f2>(-?[0-9]\d*(\.\d+)?)) (?<f3>(-?[0-9]\d*(\.\d+)?))$" >

    type FaceWTextureRegexPattern = Regex< @"(?<shape>[f]) (?<i1>(\d+))\/(?<m1>(\d+))\/(?<n1>(\d+)) (?<i2>(\d+))\/(?<m2>(\d+))\/(?<n2>(\d+)) (?<i3>(\d+))\/(?<m3>(\d+))\/(?<n3>(\d+))$" >
    type FaceWNormalsRegexPattern = Regex< @"(?<shape>[f]) (?<i1>(\d+))\/\/(?<n1>(\d+)) (?<i2>(\d+))\/\/(?<n2>(\d+)) (?<i3>(\d+))\/\/(?<n3>(\d+))$" >
    type FaceRegexPattern = Regex< @"(?<shape>[f]) (?<i1>(\d+)) (?<i2>(\d+)) (?<i3>(\d+))$" >


    type PolygonRegexPattern = Regex< @"(?<shape>[f]) (?<i1>(\d+)) (?<i2>(\d+)) (?<i3>(\d+)) (?<i4>(\d+)) (?<i5>(\d+))$" >
    type GroupRegexPattern = Regex< @"(?<group>[g]) (?<name>(\w*))$" >

    type OBJEnteties =
        | OBJVertex of Point
        | OBJNormal of Vector
        | OBJFace of i1 : int * i2:int * i3:int
        | OBJFaceWNormals of i1 : int * n1:int * i2:int * n2:int * i3:int * n3:int
        | OBJFaceWMaterial of i1 : int * m1:int * n1:int * i2:int * m2:int * n2:int * i3 : int * m3:int * n3:int
        | ObjPoly of i1 : int * i2:int * i3:int * i4:int * i5:int
        | ObjGroup of name: string


    let (|NormalRegex|_|) input =
        match NormalRegexPattern().TryTypedMatch(input) with
        | Some m -> Some (OBJNormal (Vector.create (float m.f1.Value) (float m.f2.Value) (float m.f3.Value)))
        | None -> None

    let (|VertexRegex|_|) input =
        match VertexRegexPattern().TryTypedMatch(input) with
        | Some m -> Some (OBJVertex (Point.create (float m.f1.Value) (float m.f2.Value) (float m.f3.Value)))
        | None -> None

    let (|FaceRegex|_|) input =
        match FaceRegexPattern().TryTypedMatch(input) with
        | Some m -> Some(OBJFace((int m.i1.Value), (int m.i2.Value), (int m.i3.Value)))
        | None -> None
        
    let (|FaceWNormalsRegex|_|) input =
        match FaceWNormalsRegexPattern().TryTypedMatch(input) with
        | Some m -> Some(OBJFaceWNormals((int m.i1.Value), (int m.n1.Value), (int m.i2.Value), (int m.n2.Value),(int m.i3.Value),(int m.n3.Value)))
        | None -> None
        
    let (|FaceWTextureRegex|_|) input =
        match FaceWTextureRegexPattern().TryTypedMatch(input) with
        | Some m ->
            let i1 = (int m.i1.Value)
            let n1 = (int m.n1.Value)
            let m1 = (int m.m1.Value)

            let i2 = (int m.i2.Value)
            let n2 = (int m.n2.Value)
            let m2 = (int m.m2.Value)

            let i3 = (int m.i3.Value)
            let n3 = (int m.n3.Value)
            let m3 = (int m.m3.Value)

            Some(OBJFaceWMaterial(i1, m1, n1, i2, m2, n2, i3, m3, n3))
        | None -> None

    let (|PolyRegex|_|) input =
        match PolygonRegexPattern().TryTypedMatch(input) with
        | Some m -> Some(ObjPoly((int m.i1.Value), (int m.i2.Value), (int m.i3.Value), (int m.i4.Value), (int m.i5.Value)))
        | None -> None

    
    let (|GroupRegex|_|) input =
        match GroupRegexPattern().TryTypedMatch(input) with
        | Some m ->
            Some(ObjGroup(m.Name))
        | None -> None


    let lexer (s:string []) : LexResult =
        let objEnteties =
            let parseLine s =
                match s with
                | VertexRegex m -> Some (m)
                | NormalRegex m -> Some (m)
                | FaceWTextureRegex m -> Some (m)
                | FaceWNormalsRegex m -> Some (m)
                | FaceRegex m -> Some (m)
                | PolyRegex m -> Some (m)
                | GroupRegex m -> Some (m)
                | _ -> None

            s
            |> Array.choose parseLine
            |> Array.toList

        let vertices =
            let filterVertices x =
                match x with
                | OBJVertex v -> Some v
                | _ -> None 

            objEnteties
            |> List.choose filterVertices

        let normals =
            let filterNormals x =
                match x with
                | OBJNormal v -> Some v
                | _ -> None 

            objEnteties
            |> List.choose filterNormals

        let shapes =
            let getVertexAtIndex i (vertices: OBJEnteties list) =
                match vertices.[i] with
                | OBJVertex p -> p
                | _ -> failwith "expected a vertex"

            let getNormalAtIndex i (normals: Vector list) = normals.[i]

            let filterShape (x:OBJEnteties) =
                match x with
                | OBJFaceWNormals(i1,n1,i2,n2,i3,n3) ->
                    let p1 = getVertexAtIndex (i1-1) objEnteties
                    let p2 = getVertexAtIndex (i2-1) objEnteties
                    let p3 = getVertexAtIndex (i3-1) objEnteties

                    let n1' = getNormalAtIndex (n1-1) normals
                    let n2' = getNormalAtIndex (n2-1) normals
                    let n3' = getNormalAtIndex (n3-1) normals


                    Some([Triangle.createSmooth(p1,p2,p3,n1',n2',n3')])
                | OBJFaceWMaterial(i1,m1,n1,i2,m2,n2,i3,m3,n3) ->
                    let p1 = getVertexAtIndex (i1-1) objEnteties
                    let p2 = getVertexAtIndex (i2-1) objEnteties
                    let p3 = getVertexAtIndex (i3-1) objEnteties

                    let n1' = getNormalAtIndex (n1-1) normals
                    let n2' = getNormalAtIndex (n2-1) normals
                    let n3' = getNormalAtIndex (n3-1) normals

                    Some([Triangle.createSmooth(p1,p2,p3,n1',n2',n3')])

                | OBJFace(i1,i2,i3) ->
                    let p1 = getVertexAtIndex (i1-1) objEnteties
                    let p2 = getVertexAtIndex (i2-1) objEnteties
                    let p3 = getVertexAtIndex (i3-1) objEnteties
                    Some([Triangle.create(p1,p2,p3)])
                | ObjPoly(i1,i2,i3,i4,i5) ->
                    let p1 = getVertexAtIndex (i1-1) objEnteties
                    let p2 = getVertexAtIndex (i2-1) objEnteties
                    let p3 = getVertexAtIndex (i3-1) objEnteties
                    let p4 = getVertexAtIndex (i4-1) objEnteties
                    let p5 = getVertexAtIndex (i5-1) objEnteties

                    let t1 = Triangle.create(p1,p2,p3)
                    let t2 = Triangle.create(p1,p3,p4)
                    let t3 = Triangle.create(p1,p4,p5)
                    Some ([t1;t2;t3])
                | ObjGroup m -> Some([Group.create()])
                
                | _ -> None
            

            objEnteties
            |> List.choose filterShape
            |> List.collect (fun x -> x)

        { shapes = shapes; vertices = vertices; normals = normals}

    let parseFile (s:string []) =

        let rec loop (objects:Object list) (lastGroup: Object Option) (acc:ParseResult) = 
            match objects with
            | h::t ->
                match h.shape, lastGroup with
                | Group(_), None ->
                    let groupsList = [h] @ acc.groups
                    loop t (Some h) {acc with groups = groupsList }
                | Group(_), Some g ->
                    let topGroup' = Group.addChildren [g] acc.topGroup
                    let groups' = [h] @ acc.groups;
                    let acc' = {acc with topGroup = topGroup'; groups = groups'}
                    loop t (Some h) acc'
                | (Traingle(_) | SmoothTraingle(_)), None ->
                    let topGroup' = Group.addChildren [h] acc.topGroup
                    let defaultGroup' = acc.defaultGroup @ [h]
                    let acc' = {acc with defaultGroup = defaultGroup'; topGroup = topGroup'}
                    loop t lastGroup acc'
                | (Traingle(_) | SmoothTraingle(_)), Some(group) ->
                        let group' =
                            let chlidren = Group.getChildren group
                            Group.setChildren ([h] @ chlidren) group

                        let groups' =
                            let filterd =
                                acc.groups
                                |> List.filter(fun x -> x.id <> group.id)

                            filterd @ [group']
                        let defaultGroup' = acc.defaultGroup @ [h]
                        let acc' = {acc with groups = groups'; defaultGroup = defaultGroup'}
                        loop t (Some(group')) acc'
                | _ -> failwith "error"
            | [] -> acc

        let lex = lexer s
        let parseResult = loop lex.shapes None {defaultGroup = []; groups = []; vertices = lex.vertices; topGroup = Group.create(); normals = lex.normals}
        parseResult
        //let g =
        //    List.chunkBySize 100 parseResult.defaultGroup
        //    |> List.map (fun x -> Group.create() |> Group.setChildren x)
        //{parseResult with groups = g}



