﻿Imports System.Runtime.CompilerServices
Imports RhythmBase.Events
Imports RhythmBase.Extensions
Imports RhythmBase.LevelElements
Imports RhythmBase.Components
Namespace Extensions
    Public Module Extension
        Private Function GetRange(e As OrderedEventCollection, index As Index) As (start As Single, [end] As Single)
            Dim firstEvent = e.First
            Dim lastEvent = e.Last
            Return If(index.IsFromEnd, (
                lastEvent.Beat._calculator.BarBeat_BeatOnly(lastEvent.Beat.BarBeat.bar - index.Value, 1),
                lastEvent.Beat._calculator.BarBeat_BeatOnly(lastEvent.Beat.BarBeat.bar - index.Value + 1, 1)),
                (firstEvent.Beat._calculator.BarBeat_BeatOnly(index.Value, 1),
                firstEvent.Beat._calculator.BarBeat_BeatOnly(index.Value + 1, 1)))
        End Function
        Private Function GetRange(e As OrderedEventCollection, range As Range) As (start As Single, [end] As Single)
            Dim firstEvent = e.First
            Dim lastEvent = e.Last
            Return (If(range.Start.IsFromEnd,
                lastEvent.Beat._calculator.BarBeat_BeatOnly(lastEvent.Beat.BarBeat.bar - range.Start.Value, 1),
                firstEvent.Beat._calculator.BarBeat_BeatOnly(range.Start.Value, 1)),
                If(range.End.IsFromEnd,
                lastEvent.Beat._calculator.BarBeat_BeatOnly(lastEvent.Beat.BarBeat.bar - range.End.Value + 1, 1),
                firstEvent.Beat._calculator.BarBeat_BeatOnly(range.End.Value + 1, 1)))
        End Function
        <Extension> Public Function IsNullOrEmpty(e As String) As Boolean
            Return e Is Nothing OrElse e.Length = 0
        End Function
        <Extension> Public Function UpperCamelCase(e As String) As String
            Dim S = e.ToArray
            S(0) = S(0).ToString.ToUpper
            Return String.Join("", S)
        End Function
        <Extension> Public Function LowerCamelCase(e As String) As String
            Dim S = e.ToArray
            S(0) = S(0).ToString.ToLower
            Return String.Join("", S)
        End Function
        <Extension> Public Function RgbaToArgb(Rgba As Int32) As Int32
            Return ((Rgba >> 8) And &HFFFFFF) Or ((Rgba << 24) And &HFF000000)
        End Function
        <Extension> Public Function ArgbToRgba(Argb As Int32) As Int32
            Return ((Argb >> 24) And &HFF) Or ((Argb << 8) And &HFFFFFF00)
        End Function
        <Extension> Public Function ToRDPoint(e As SkiaSharp.SKPoint) As RDPointN
            Return New RDPointN(e.X, e.Y)
        End Function
        <Extension> Public Function ToRDPointI(e As SkiaSharp.SKPointI) As RDPointNI
            Return New RDPointNI(e.X, e.Y)
        End Function
        <Extension> Public Function ToRDSize(e As SkiaSharp.SKSize) As RDSizeN
            Return New RDSizeN(e.Width, e.Height)
        End Function
        <Extension> Public Function ToRDSizeI(e As SkiaSharp.SKSizeI) As RDSizeNI
            Return New RDSizeNI(e.Width, e.Height)
        End Function
        <Extension> Public Function ToSKPoint(e As RDPointN) As SkiaSharp.SKPoint
            Return New SkiaSharp.SKPoint(e.X, e.Y)
        End Function
        <Extension> Public Function ToSKPointI(e As RDPointNI) As SkiaSharp.SKPointI
            Return New SkiaSharp.SKPointI(e.X, e.Y)
        End Function
        <Extension> Public Function ToSKSize(e As RDSizeN) As SkiaSharp.SKSize
            Return New SkiaSharp.SKSize(e.Width, e.Height)
        End Function
        <Extension> Public Function ToSKSizeI(e As RDSizeNI) As SkiaSharp.SKSizeI
            Return New SkiaSharp.SKSizeI(e.Width, e.Height)
        End Function
        <Extension> Public Function FixFraction(number As Single, splitBase As UInteger) As Single
            Return Math.Round(number * splitBase) / splitBase
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean)) As IEnumerable(Of T)
            Return e.EventsBeatOrder.SelectMany(Function(i) i.Value).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), beat As Single) As IEnumerable(Of T)
            Dim value As List(Of BaseEvent) = Nothing
            If e.EventsBeatOrder.TryGetValue(beat, value) Then
                Return value
            End If
            Return value
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), startBeat As Single, endBeat As Single) As IEnumerable(Of T)
            Return e.EventsBeatOrder.TakeWhile(Function(i) i.Key < endBeat).SkipWhile(Function(i) i.Key < startBeat).SelectMany(Function(i) i.Value)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), beat As RDBeat) As IEnumerable(Of T)
            Dim value As List(Of BaseEvent) = Nothing
            If e.EventsBeatOrder.TryGetValue(beat.BeatOnly, value) Then
                Return value
            End If
            Return value
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), startBeat As RDBeat, endBeat As RDBeat) As IEnumerable(Of T)
            Return e.Where(startBeat.BeatOnly, endBeat.BeatOnly)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), index As Index) As IEnumerable(Of T)
            Dim rg = GetRange(e, index)
            Return e.Where(rg.start, rg.end)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), range As RDRange) As IEnumerable(Of T)
            Return e.EventsBeatOrder.TakeWhile(Function(i) If(i.Key < range.End?.BeatOnly, True)).SkipWhile(Function(i) If(i.Key < range.Start?.BeatOnly, False)).SelectMany(Function(i) i.Value)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), range As Range) As IEnumerable(Of T)
            Dim rg = GetRange(e, range)
            Return e.Where(rg.start, rg.end)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), beat As Single) As IEnumerable(Of T)
            Return e.Where(beat).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), startBeat As Single, endBeat As Single) As IEnumerable(Of T)
            Return e.Where(startBeat, endBeat).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), beat As RDBeat) As IEnumerable(Of T)
            Return e.Where(beat).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), startBeat As RDBeat, endBeat As RDBeat) As IEnumerable(Of T)
            Return e.Where(startBeat, endBeat).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), range As RDRange) As IEnumerable(Of T)
            Return e.Where(range).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), index As Index) As IEnumerable(Of T)
            Return e.Where(index).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), range As Range) As IEnumerable(Of T)
            Return e.Where(range).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection) As IEnumerable(Of T)
            Return e.OfType(Of T)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, beat As Single) As IEnumerable(Of T)
            Dim value As List(Of BaseEvent) = Nothing
            If e.EventsBeatOrder.TryGetValue(beat, value) Then
                Return value.OfType(Of T)
            End If
            Return value
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, startBeat As Single, endBeat As Single) As IEnumerable(Of T)
            Return e.EventsBeatOrder.TakeWhile(Function(i) i.Key < endBeat).SkipWhile(Function(i) i.Key < startBeat).SelectMany(Function(i) i.Value.OfType(Of T))
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, beat As RDBeat) As IEnumerable(Of T)
            Dim value As List(Of BaseEvent) = Nothing
            If e.EventsBeatOrder.TryGetValue(beat.BeatOnly, value) Then
                Return value.OfType(Of T)
            End If
            Return value
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, startBeat As RDBeat, endBeat As RDBeat) As IEnumerable(Of T)
            Return e.Where(Of T)(startBeat.BeatOnly, endBeat.BeatOnly)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, index As Index) As IEnumerable(Of T)
            Dim rg = GetRange(e, index)
            Return e.Where(Of T)(rg.start, rg.end)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, range As RDRange) As IEnumerable(Of T)
            Return e.EventsBeatOrder.TakeWhile(Function(i) If(i.Key < range.End?.BeatOnly, True)).SkipWhile(Function(i) If(i.Key < range.Start?.BeatOnly, False)).SelectMany(Function(i) i.Value.OfType(Of T))
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, range As Range) As IEnumerable(Of T)
            Dim rg = GetRange(e, range)
            Return e.Where(Of T)(rg.start, rg.end)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean)) As IEnumerable(Of T)
            Return e.Where(Of T)().Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), beat As Single) As IEnumerable(Of T)
            Return e.Where(Of T)(beat).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), startBeat As Single, endBeat As Single) As IEnumerable(Of T)
            Return e.Where(Of T)(startBeat, endBeat).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), beat As RDBeat) As IEnumerable(Of T)
            Return e.Where(Of T)(beat).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), startBeat As RDBeat, endBeat As RDBeat) As IEnumerable(Of T)
            Return e.Where(Of T)(startBeat, endBeat).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), range As RDRange) As IEnumerable(Of T)
            Return e.Where(Of T)(range).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), index As Index) As IEnumerable(Of T)
            Return e.Where(Of T)(index).Where(predicate)
        End Function
        <Extension> Public Function Where(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), range As Range) As IEnumerable(Of T)
            Return e.Where(Of T)(range).Where(predicate)
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean)) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), beat As Single) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(beat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), startBeat As Single, endBeat As Single) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(startBeat, endBeat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), beat As RDBeat) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(beat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), startBeat As RDBeat, endBeat As RDBeat) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(startBeat, endBeat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), index As Index) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(index)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), range As RDRange) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(range)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), range As Range) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(range)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), beat As Single) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, beat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), startBeat As Single, endBeat As Single) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, startBeat, endBeat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), beat As RDBeat) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, beat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), startBeat As RDBeat, endBeat As RDBeat) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, startBeat, endBeat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), range As RDRange) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, range)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), index As Index) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, index)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), range As Range) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, range)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)()))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, beat As Single) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(beat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, startBeat As Single, endBeat As Single) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(startBeat, endBeat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, beat As RDBeat) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(beat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, startBeat As RDBeat, endBeat As RDBeat) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(startBeat, endBeat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, range As RDRange) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(range)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, index As Index) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(index)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, range As Range) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(range)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean)) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), beat As Single) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, beat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), startBeat As Single, endBeat As Single) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, startBeat, endBeat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), beat As RDBeat) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, beat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), startBeat As RDBeat, endBeat As RDBeat) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, startBeat, endBeat)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), range As RDRange) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, range)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), index As Index) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, index)))
        End Function
        <Extension> Public Function RemoveAll(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), range As Range) As Integer
            Return e.RemoveRange(New List(Of T)(e.Where(Of T)(predicate, range)))
        End Function
        <Extension> Public Function First(Of T As BaseEvent)(e As OrderedEventCollection(Of T)) As T
            Return e.EventsBeatOrder.First.Value.First
        End Function
        <Extension> Public Function First(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean)) As T
            Return e.ConcatAll.First(predicate)
        End Function
        <Extension> Public Function First(Of T As BaseEvent)(e As OrderedEventCollection) As T
            Return e.Where(Of T).First
        End Function
        <Extension> Public Function First(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean)) As T
            Return e.Where(Of T).First(predicate)
        End Function
        <Extension> Public Function FirstOrDefault(Of T As BaseEvent)(e As OrderedEventCollection(Of T)) As T
            Return e.EventsBeatOrder.FirstOrDefault.Value?.FirstOrDefault
        End Function
        <Extension> Public Function FirstOrDefault(Of T As BaseEvent)(e As OrderedEventCollection(Of T), defaultValue As T) As T
            Return e.ConcatAll.FirstOrDefault(defaultValue)
        End Function
        <Extension> Public Function FirstOrDefault(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean)) As T
            Return e.ConcatAll.FirstOrDefault(predicate)
        End Function
        <Extension> Public Function FirstOrDefault(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), defaultValue As T) As T
            Return e.ConcatAll.FirstOrDefault(predicate, defaultValue)
        End Function
        <Extension> Public Function FirstOrDefault(Of T As BaseEvent)(e As OrderedEventCollection) As T
            Return e.Where(Of T).FirstOrDefault()
        End Function
        <Extension> Public Function FirstOrDefault(Of T As BaseEvent)(e As OrderedEventCollection, defaultValue As T) As T
            Return e.Where(Of T).FirstOrDefault(defaultValue)
        End Function
        <Extension> Public Function FirstOrDefault(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean)) As T
            Return e.Where(Of T).FirstOrDefault(predicate)
        End Function
        <Extension> Public Function FirstOrDefault(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), defaultValue As T) As T
            Return e.Where(Of T).FirstOrDefault(predicate, defaultValue)
        End Function
        <Extension> Public Function Last(Of T As BaseEvent)(e As OrderedEventCollection(Of T)) As T
            Return e.EventsBeatOrder.Last.Value.Last
        End Function
        <Extension> Public Function Last(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean)) As T
            Return e.ConcatAll.Last(predicate)
        End Function
        <Extension> Public Function Last(Of T As BaseEvent)(e As OrderedEventCollection) As T
            Return e.Where(Of T).Last
        End Function
        <Extension> Public Function Last(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean)) As T
            Return e.Where(Of T).Last(predicate)
        End Function
        <Extension> Public Function LastOrDefault(Of T As BaseEvent)(e As OrderedEventCollection(Of T)) As T
            Return e.EventsBeatOrder.LastOrDefault.Value?.LastOrDefault()
        End Function
        <Extension> Public Function LastOrDefault(Of T As BaseEvent)(e As OrderedEventCollection(Of T), defaultValue As T) As T
            Return e.ConcatAll.LastOrDefault(defaultValue)
        End Function
        <Extension> Public Function LastOrDefault(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean)) As T
            Return e.ConcatAll.LastOrDefault(predicate)
        End Function
        <Extension> Public Function LastOrDefault(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), defaultValue As T) As T
            Return e.ConcatAll.LastOrDefault(predicate, defaultValue)
        End Function
        <Extension> Public Function LastOrDefault(Of T As BaseEvent)(e As OrderedEventCollection) As T
            Return e.Where(Of T).LastOrDefault()
        End Function
        <Extension> Public Function LastOrDefault(Of T As BaseEvent)(e As OrderedEventCollection, defaultValue As T) As T
            Return e.Where(Of T).LastOrDefault(defaultValue)
        End Function
        <Extension> Public Function LastOrDefault(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean)) As T
            Return e.Where(Of T).LastOrDefault(predicate)
        End Function
        <Extension> Public Function LastOrDefault(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), defaultValue As T) As T
            Return e.Where(Of T).LastOrDefault(predicate, defaultValue)
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection(Of T), beat As Single) As IEnumerable(Of T)
            Return e.EventsBeatOrder.TakeWhile(Function(i) i.Key < beat).SelectMany(Function(i) i.Value)
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection(Of T), beat As RDBeat) As IEnumerable(Of T)
            Return e.TakeWhile(beat.BeatOnly)
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection(Of T), index As Index) As IEnumerable(Of T)
            Dim firstEvent = e.First
            Dim lastEvent = e.Last
            Return e.TakeWhile(
If(index.IsFromEnd,
lastEvent.Beat._calculator.BarBeat_BeatOnly(lastEvent.Beat.BarBeat.bar - index.Value + 1, 1),
firstEvent.Beat._calculator.BarBeat_BeatOnly(index.Value + 1, 1)))
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean)) As IEnumerable(Of T)
            Return e.EventsBeatOrder.SelectMany(Function(i) i.Value).TakeWhile(predicate)
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), beat As Single) As IEnumerable(Of T)
            Return e.TakeWhile(beat).TakeWhile(predicate)
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), beat As RDBeat) As IEnumerable(Of T)
            Return e.TakeWhile(beat).TakeWhile(predicate)
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection(Of T), predicate As Func(Of T, Boolean), index As Index) As IEnumerable(Of T)
            Return e.TakeWhile(index).TakeWhile(predicate)
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection, beat As Single) As IEnumerable(Of T)
            Return e.EventsBeatOrder.TakeWhile(Function(i) i.Key < beat).SelectMany(Function(i) i.Value.OfType(Of T))
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection, beat As RDBeat) As IEnumerable(Of T)
            Return e.TakeWhile(Of T)(beat.BeatOnly)
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection, index As Index) As IEnumerable(Of T)
            Dim firstEvent = e.First
            Dim lastEvent = e.Last
            Return e.TakeWhile(Of T)(
If(index.IsFromEnd,
lastEvent.Beat._calculator.BarBeat_BeatOnly(lastEvent.Beat.BarBeat.bar - index.Value + 1, 1),
firstEvent.Beat._calculator.BarBeat_BeatOnly(index.Value + 1, 1)))
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean)) As IEnumerable(Of T)
            Return e.EventsBeatOrder.SelectMany(Function(i) i.Value.OfType(Of T))
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), beat As Single) As IEnumerable(Of T)
            Return e.TakeWhile(Of T)(beat).TakeWhile(predicate)
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), beat As RDBeat) As IEnumerable(Of T)
            Return e.TakeWhile(Of T)(beat).TakeWhile(predicate)
        End Function
        <Extension> Public Function TakeWhile(Of T As BaseEvent)(e As OrderedEventCollection, predicate As Func(Of T, Boolean), index As Index) As IEnumerable(Of T)
            Return e.TakeWhile(Of T)(index).TakeWhile(predicate)
        End Function
        <Extension> Public Function RemoveRange(Of T As BaseEvent)(e As OrderedEventCollection, items As IEnumerable(Of T)) As Integer
            Dim count As Integer = 0
            For Each item In items
                count += e.Remove(item)
            Next
            Return count
        End Function
        <Extension> Public Function RemoveRange(Of T As BaseEvent)(e As OrderedEventCollection(Of T), items As IEnumerable(Of T)) As Integer
            Dim count As Integer = 0
            For Each item In items
                count += e.Remove(item)
            Next
            Return count
        End Function
        <Extension> Public Function IsInFrontOf(Of T As BaseEvent)(e As OrderedEventCollection(Of T), item1 As T, item2 As T) As Boolean
            Return item1.Beat < item2.Beat OrElse
(item1.Beat.BeatOnly = item2.Beat.BeatOnly AndAlso
e.EventsBeatOrder(item1.Beat.BeatOnly).IndexOf(item1) < e.EventsBeatOrder(item2.Beat.BeatOnly).IndexOf(item2))
        End Function
        <Extension> Public Function IsBehind(Of T As BaseEvent)(e As OrderedEventCollection(Of T), item1 As T, item2 As T) As Boolean
            Return item1.Beat > item2.Beat OrElse
(item1.Beat.BeatOnly = item2.Beat.BeatOnly AndAlso
e.EventsBeatOrder(item1.Beat.BeatOnly).IndexOf(item1) > e.EventsBeatOrder(item2.Beat.BeatOnly).IndexOf(item2))
        End Function
        <Extension> Public Function GetHitBeat(e As RDLevel) As IEnumerable(Of Hit)
            Dim L As New List(Of Hit)
            For Each item In e.Rows
                L.AddRange(item.HitBeats)
            Next
            Return L
        End Function
        <Extension> Public Function GetHitEvents(e As RDLevel) As IEnumerable(Of BaseBeat)
            Return e.Where(Of BaseBeat)(Function(i) i.Hitable)
        End Function
        <Extension> Public Function GetTaggedEvents(Of T As BaseEvent)(e As OrderedEventCollection(Of T), name As String, direct As Boolean) As IEnumerable(Of IGrouping(Of String, T))
            If name Is Nothing Then
                Return Nothing
            End If
            If direct Then
                Return e.Where(Function(i) i.Tag = name).GroupBy(Function(i) i.Tag)
            Else
                Return e.Where(Function(i) If(i.Tag?.Contains(name), False)).GroupBy(Function(i) i.Tag)
            End If
        End Function
        <Extension> Private Function ClassicBeats(e As Row) As IEnumerable(Of BaseBeat)
            Return e.Where(Of BaseBeat)(Function(i) (i.Type = EventType.AddClassicBeat Or
                                i.Type = EventType.AddFreeTimeBeat Or
                                i.Type = EventType.PulseFreeTimeBeat) AndAlso
                                i.Hitable)
        End Function
        <Extension> Private Function OneshotBeats(e As Row) As IEnumerable(Of BaseBeat)
            Return e.Where(Of BaseBeat)(Function(i) i.Type = EventType.AddOneshotBeat AndAlso
                                i.Hitable)
        End Function
        <Extension> Public Function HitBeats(e As Row) As IEnumerable(Of Hit)
            Select Case e.RowType
                Case RowType.Classic
                    Return e.ClassicBeats().SelectMany(Function(i) i.HitTimes)
                Case RowType.Oneshot
                    Return e.OneshotBeats().SelectMany(Function(i) i.HitTimes)
                Case Else
                    Throw New Exceptions.RhythmBaseException("How?")
            End Select
        End Function
        <Extension> Public Function GetRowBeatStatus(e As Row) As SortedDictionary(Of Single, Integer())
            Dim L As New SortedDictionary(Of Single, Integer())
            Select Case e.RowType
                Case RowType.Classic
                    Dim value As Integer() = New Integer(6) {}
                    L.Add(0, value)
                    For Each beat In e
                        Select Case beat.Type
                            Case EventType.AddClassicBeat
                                Dim trueBeat = CType(beat, AddClassicBeat)
                                For i = 0 To 6
                                    Dim statusArray As Integer() = If(L(beat.Beat.BeatOnly), New Integer(6) {})
                                    statusArray(i) += 1
                                    L(beat.Beat.BeatOnly) = statusArray
                                Next
                            Case EventType.AddFreeTimeBeat
                            Case EventType.PulseFreeTimeBeat
                            Case EventType.SetRowXs
                        End Select
                    Next
                Case RowType.Oneshot
                Case Else
                    Throw New Exceptions.RhythmBaseException("How")
            End Select
            Return L
        End Function
        <Extension> Public Function Beats(e As Row) As IEnumerable(Of BaseBeat)
            Select Case e.RowType
                Case RowType.Classic
                    Return e.ClassicBeats()
                Case RowType.Oneshot
                    Return e.OneshotBeats()
                Case Else
                    Throw New Exceptions.RhythmBaseException("How?")
            End Select
        End Function
    End Module
    Public Module EventsExtension
        Public Enum wavetype
            BoomAndRush
            Spring
            Spike
            SpikeHuge
            Ball
            [Single]
        End Enum
        Public Enum ShockWaveType
            size
            distortion
            duration
        End Enum
        Public Enum Particle
            HitExplosion
            leveleventexplosion
        End Enum
        Public Structure ProceduralTree
            Public brachedPerlteration As Single?
            Public branchesPerDivision As Single?
            Public iterationsPerSecond As Single?
            Public thickness As Single?
            Public targetLength As Single?
            Public maxDeviation As Single?
            Public angle As Single?
            Public camAngle As Single?
            Public camDistance As Single?
            Public camDegreesPerSecond As Single?
            Public camSpeed As Single?
            Public pulseIntensity As Single?
            Public pulseRate As Single?
            Public pulseWavelength As Single?
        End Structure
        <Extension> Public Function IsInFrontOf(e As BaseEvent, item As BaseEvent) As Boolean
            Return e.Beat.baseLevel.IsInFrontOf(e, item)
        End Function
        <Extension> Public Function IsBehind(e As BaseEvent, item As BaseEvent) As Boolean
            Return e.Beat.baseLevel.IsBehind(e, item)
        End Function
        '<Extension> Public Sub MovePositionMaintainVisual(e As Move, target As RDPointE)
        '    If e.Position Is Nothing OrElse e.Pivot Is Nothing OrElse e.Angle Is Nothing OrElse Not e.Angle.Value.IsNumeric Then
        '        Exit Sub
        '    End If
        '    e.Position = target
        '    e.Pivot = (e.VisualPosition() - New RDSizeE(target)).Rotate(e.Angle.Value.NumericValue)
        'End Sub
        '<Extension> Public Sub MovePositionMaintainVisual(e As MoveRoom, target As RDSizeE)
        '    If e.RoomPosition Is Nothing OrElse e.Pivot Is Nothing OrElse e.Angle Is Nothing OrElse Not e.Angle.Value.IsNumeric Then
        '        Exit Sub
        '    End If
        '    e.RoomPosition = target
        '    e.Pivot = (e.VisualPosition() - New RDSizeE(target)).Rotate(e.Angle.Value.NumericValue)
        'End Sub
        '<Extension> Public Function VisualPosition(e As Move) As RDPointE
        '    If e.Position Is Nothing OrElse e.Pivot Is Nothing OrElse e.Angle Is Nothing OrElse Not e.Angle.Value.IsNumeric OrElse e.Scale Is Nothing Then
        '        Return New RDPoint
        '    End If
        '    Dim previousPosition As RDPointE = e.Position
        '    Dim previousPivot As RDPointE = (e.Pivot * (e.Scale / 100) * e.Parent.Size)
        '    Return previousPosition + New RDSizeE(previousPivot.Rotate(e.Angle.Value.NumericValue))
        'End Function
        '<Extension> Public Function VisualPosition(e As MoveRoom) As RDPointE
        '    If e.RoomPosition Is Nothing OrElse e.Pivot Is Nothing OrElse e.Angle Is Nothing Then
        '        Return New RDPoint
        '    End If
        '    Dim previousPosition As RDPointE = e.RoomPosition
        '    Dim previousPivot As RDPointE = e.Pivot * e.Scale
        '    Return previousPosition + New RDSizeE(previousPivot.Rotate(e.Angle.Value.NumericValue))
        'End Function
        <Extension> Public Sub SetScoreboardLights(e As CallCustomMethod, Mode As Boolean, Text As String)
            e.MethodName = $"{NameOf(SetScoreboardLights)}({Mode},{Text})"
        End Sub
        <Extension> Public Sub InvisibleChars(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(InvisibleChars).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub InvisibleHeart(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(InvisibleHeart).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub NoHitFlashBorder(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(NoHitFlashBorder).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub NoHitStrips(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(NoHitStrips).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub SetOneshotType(e As CallCustomMethod, rowID As Integer, wavetype As ShockWaveType)
            e.MethodName = $"{NameOf(SetOneshotType)}({rowID},str:{wavetype})"
        End Sub
        <Extension> Public Sub WobblyLines(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(WobblyLines).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub TrueCameraMove(e As Comment, RoomID As Integer, p As RDPoint, AnimationDuration As Single, Ease As EaseType)
            e.Text = $"()=>{NameOf(TrueCameraMove).LowerCamelCase}({RoomID},{p.X},{p.Y},{AnimationDuration},{Ease})"
        End Sub
        <Extension> Public Sub Create(e As Comment, particleName As Particle, p As RDPoint)
            e.Text = $"()=>{NameOf(Create).LowerCamelCase}(CustomParticles/{particleName},{p.X},{p.Y})"
        End Sub
        <Extension> Public Sub ShockwaveSizeMultiplier(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(ShockwaveSizeMultiplier).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub ShockwaveDistortionMultiplier(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(ShockwaveDistortionMultiplier).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub ShockwaveDurationMultiplier(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(ShockwaveDurationMultiplier).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub Shockwave(e As Comment, type As ShockWaveType, value As Single)
            e.Text = $"()=>{NameOf(Shockwave).LowerCamelCase}({type},{value})"
        End Sub
        <Extension> Public Sub MistakeOrHeal(e As CallCustomMethod, damageOrHeal As Single)
            e.MethodName = $"{NameOf(MistakeOrHeal)}({damageOrHeal})"
        End Sub
        <Extension> Public Sub MistakeOrHealP1(e As CallCustomMethod, damageOrHeal As Single)
            e.MethodName = $"{NameOf(MistakeOrHealP1)}({damageOrHeal})"
        End Sub
        <Extension> Public Sub MistakeOrHealP2(e As CallCustomMethod, damageOrHeal As Single)
            e.MethodName = $"{NameOf(MistakeOrHealP2)}({damageOrHeal})"
        End Sub
        <Extension> Public Sub MistakeOrHealSilent(e As CallCustomMethod, damageOrHeal As Single)
            e.MethodName = $"{NameOf(MistakeOrHealSilent)}({damageOrHeal})"
        End Sub
        <Extension> Public Sub MistakeOrHealP1Silent(e As CallCustomMethod, damageOrHeal As Single)
            e.MethodName = $"{NameOf(MistakeOrHealP1Silent)}({damageOrHeal})"
        End Sub
        <Extension> Public Sub MistakeOrHealP2Silent(e As CallCustomMethod, damageOrHeal As Single)
            e.MethodName = $"{NameOf(MistakeOrHealP2Silent)}({damageOrHeal})"
        End Sub
        <Extension> Public Sub SetMistakeWeight(e As CallCustomMethod, rowID As Integer, weight As Single)
            e.MethodName = $"{NameOf(SetMistakeWeight)}({rowID},{weight})"
        End Sub
        <Extension> Public Sub DamageHeart(e As CallCustomMethod, rowID As Integer, damage As Single)
            e.MethodName = $"{NameOf(DamageHeart)}({rowID},{damage})"
        End Sub
        <Extension> Public Sub HealHeart(e As CallCustomMethod, rowID As Integer, damage As Single)
            e.MethodName = $"{NameOf(HealHeart)}({rowID},{damage})"
        End Sub
        <Extension> Public Sub WavyRowsAmplitude(e As CallCustomMethod, roomID As Byte, amplitude As Single)
            e.MethodName = $"room[{roomID}].{NameOf(WavyRowsAmplitude).LowerCamelCase} = {amplitude}"
        End Sub
        <Extension> Public Sub WavyRowsAmplitude(e As Comment, roomID As Byte, amplitude As Single, duration As Single)
            e.Text = $"()=>{NameOf(WavyRowsAmplitude).LowerCamelCase}({roomID},{amplitude},{duration})"
        End Sub
        <Extension> Public Sub WavyRowsFrequency(e As CallCustomMethod, roomID As Byte, frequency As Single)
            e.MethodName = $"room[{roomID}].{NameOf(WavyRowsFrequency).LowerCamelCase} = {frequency}"
        End Sub
        <Extension> Public Sub SetShakeIntensityOnHit(e As CallCustomMethod, roomID As Byte, number As Integer, strength As Integer)
            e.MethodName = $"room[{roomID}].{NameOf(SetShakeIntensityOnHit)}({number},{strength})"
        End Sub
        <Extension> Public Sub ShowPlayerHand(e As CallCustomMethod, roomID As Byte, isPlayer1 As Boolean, isShortArm As Boolean, isInstant As Boolean)
            e.MethodName = $"{NameOf(ShowPlayerHand)}({roomID},{isPlayer1},{isShortArm},{isInstant})"
        End Sub
        <Extension> Public Sub TintHandsWithInts(e As CallCustomMethod, roomID As Byte, R As Single, G As Single, B As Single, A As Single)
            e.MethodName = $"{NameOf(TintHandsWithInts)}({roomID},{R},{G},{B},{A})"
        End Sub
        <Extension> Public Sub SetHandsBorderColor(e As CallCustomMethod, roomID As Byte, R As Single, G As Single, B As Single, A As Single, style As Integer)
            e.MethodName = $"{NameOf(SetHandsBorderColor)}({roomID},{R},{G},{B},{A},{style})"
        End Sub
        <Extension> Public Sub SetAllHandsBorderColor(e As CallCustomMethod, R As Single, G As Single, B As Single, A As Single, style As Integer)
            e.MethodName = $"{NameOf(SetAllHandsBorderColor)}({R},{G},{B},{A},{style})"
        End Sub
        <Extension> Public Sub SetHandToP1(e As CallCustomMethod, room As Integer, rightHand As Boolean)
            e.MethodName = $"{NameOf(SetHandToP1)}({room},{rightHand})"
        End Sub
        <Extension> Public Sub SetHandToP2(e As CallCustomMethod, room As Integer, rightHand As Boolean)
            e.MethodName = $"{NameOf(SetHandToP2)}({room},{rightHand})"
        End Sub
        <Extension> Public Sub SetHandToIan(e As CallCustomMethod, room As Integer, rightHand As Boolean)
            e.MethodName = $"{NameOf(SetHandToIan)}({room},{rightHand})"
        End Sub
        <Extension> Public Sub SetHandToPaige(e As CallCustomMethod, room As Integer, rightHand As Boolean)
            e.MethodName = $"{NameOf(SetHandToPaige)}({room},{rightHand})"
        End Sub
        <Extension> Public Sub SetShadowRow(e As CallCustomMethod, mimickerRowID As Integer, mimickedRowID As Integer)
            e.MethodName = $"{NameOf(SetShadowRow)}({mimickerRowID},{mimickedRowID})"
        End Sub
        <Extension> Public Sub UnsetShadowRow(e As CallCustomMethod, mimickerRowID As Integer, mimickedRowID As Integer)
            e.MethodName = $"{NameOf(UnsetShadowRow)}({mimickerRowID},{mimickedRowID})"
        End Sub
        <Extension> Public Sub ShakeCam(e As CallCustomMethod, number As Integer, strength As Integer, roomID As Integer)
            e.MethodName = $"vfx.{NameOf(ShakeCam)}({number},{strength},{roomID})"
        End Sub
        <Extension> Public Sub StopShakeCam(e As CallCustomMethod, roomID As Integer)
            e.MethodName = $"vfx.{NameOf(StopShakeCam)}({roomID})"
        End Sub
        <Extension> Public Sub ShakeCamSmooth(e As CallCustomMethod, duration As Integer, strength As Integer, roomID As Integer)
            e.MethodName = $"vfx.{NameOf(ShakeCamSmooth)}({duration},{strength},{roomID})"
        End Sub
        <Extension> Public Sub ShakeCamRotate(e As CallCustomMethod, duration As Integer, strength As Integer, roomID As Integer)
            e.MethodName = $"vfx.{NameOf(ShakeCamRotate)}({duration},{strength},{roomID})"
        End Sub
        <Extension> Public Sub SetKaleidoscopeColor(e As CallCustomMethod, roomID As Integer, R1 As Single, G1 As Single, B1 As Single, R2 As Single, G2 As Single, B2 As Single)
            e.MethodName = $"{NameOf(SetKaleidoscopeColor)}({roomID},{R1},{G1},{B1},{R2},{G2},{B2})"
        End Sub
        <Extension> Public Sub SyncKaleidoscopes(e As CallCustomMethod, targetRoomID As Integer, otherRoomID As Integer)
            e.MethodName = $"{NameOf(SyncKaleidoscopes)}({targetRoomID},{otherRoomID})"
        End Sub
        <Extension> Public Sub SetVignetteAlpha(e As CallCustomMethod, alpha As Single, roomID As Integer)
            e.MethodName = $"vfx.{NameOf(SetVignetteAlpha)}({alpha},{roomID})"
        End Sub
        <Extension> Public Sub NoOneshotShadows(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(NoOneshotShadows).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Function TextOnly(e As ShowDialogue) As String
            Dim result = e.Text
            For Each item In {
                "shake",
                "shakeRadius=\d+",
                "wave",
                "waveHeight=\d+",
                "waveSpeed=\d+",
                "swirl",
                "swirlRadius=\d+",
                "swirlSpeed=\d+",
                "static"
            }
                result = Text.RegularExpressions.Regex.Replace(result, $"\[{item}\]", "")
            Next
            Return result
        End Function
        <Extension> Public Sub EnableRowReflections(e As CallCustomMethod, roomID As Integer)
            e.MethodName = $"{NameOf(EnableRowReflections)}({roomID})"
        End Sub
        <Extension> Public Sub DisableRowReflections(e As CallCustomMethod, roomID As Integer)
            e.MethodName = $"{NameOf(DisableRowReflections)}({roomID})"
        End Sub
        <Extension> Public Sub ChangeCharacter(e As CallCustomMethod, Name As String, roomID As Integer)
            e.MethodName = $"{NameOf(ChangeCharacter)}(str:{Name},{roomID})"
        End Sub
        <Extension> Public Sub ChangeCharacter(e As CallCustomMethod, Name As Characters, roomID As Integer)
            e.MethodName = $"{NameOf(ChangeCharacter)}(str:{Name},{roomID})"
        End Sub
        <Extension> Public Sub ChangeCharacterSmooth(e As CallCustomMethod, Name As String, roomID As Integer)
            e.MethodName = $"{NameOf(ChangeCharacterSmooth)}(str:{Name},{roomID})"
        End Sub
        <Extension> Public Sub ChangeCharacterSmooth(e As CallCustomMethod, Name As Characters, roomID As Integer)
            e.MethodName = $"{NameOf(ChangeCharacterSmooth)}(str:{Name},{roomID})"
        End Sub
        <Extension> Public Sub SmoothShake(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(SmoothShake).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub RotateShake(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(RotateShake).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub DisableRowChangeWarningFlashes(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(DisableRowChangeWarningFlashes).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub StatusSignWidth(e As CallCustomMethod, value As Single)
            e.MethodName = $"{NameOf(StatusSignWidth).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub SkippableRankScreen(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(SkippableRankScreen).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub MissesToCrackHeart(e As CallCustomMethod, value As Integer)
            e.MethodName = $"{NameOf(MissesToCrackHeart).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub SkipRankText(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(SkipRankText).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub AlternativeMatrix(e As CallCustomMethod, value As Boolean)
            e.MethodName = $"{NameOf(AlternativeMatrix).LowerCamelCase} = {value}"
        End Sub
        <Extension> Public Sub ToggleSingleRowReflections(e As CallCustomMethod, room As Byte, row As Byte, value As Boolean)
            e.MethodName = $"{NameOf(ToggleSingleRowReflections)}({room},{row},{value})"
        End Sub
        <Extension> Public Sub SetScrollSpeed(e As CallCustomMethod, roomID As Byte, speed As Single, duration As Single, ease As EaseType)
            e.MethodName = $"room[{roomID}].{NameOf(SetScrollSpeed)}({speed},{duration},str:{ease})"
        End Sub
        <Extension> Public Sub SetScrollOffset(e As CallCustomMethod, roomID As Byte, cameraOffset As Single, duration As Single, ease As EaseType)
            e.MethodName = $"room[{roomID}].{NameOf(SetScrollOffset)}({cameraOffset},{duration},str:{ease})"
        End Sub
        <Extension> Public Sub DarkenedRollerdisco(e As CallCustomMethod, roomID As Byte, value As Single)
            e.MethodName = $"room[{roomID}].{NameOf(DarkenedRollerdisco)}({value})"
        End Sub
        <Extension> Public Sub CurrentSongVol(e As CallCustomMethod, targetVolume As Single, fadeTimeSeconds As Single)
            e.MethodName = $"{NameOf(CurrentSongVol)}({targetVolume},{fadeTimeSeconds})"
        End Sub
        <Extension> Public Sub PreviousSongVol(e As CallCustomMethod, targetVolume As Single, fadeTimeSeconds As Single)
            e.MethodName = $"{NameOf(PreviousSongVol)}({targetVolume},{fadeTimeSeconds})"
        End Sub
        <Extension> Public Sub EditTree(e As CallCustomMethod, room As Byte, [property] As String, value As Single, beats As Single, ease As EaseType)
            e.MethodName = $"room[{room}].{NameOf(EditTree)}(""{[property]}"",{value},{beats},""{ease}"")"
        End Sub
        <Extension> Public Function EditTree(e As CallCustomMethod, room As Byte, treeProperties As ProceduralTree, beats As Single, ease As EaseType) As IEnumerable(Of CallCustomMethod)
            Dim L As New List(Of CallCustomMethod)
            For Each p In GetType(ProceduralTree).GetFields
                If p.GetValue(treeProperties) IsNot Nothing Then
                    Dim T As New CallCustomMethod
                    T.EditTree(room, p.Name, p.GetValue(treeProperties), beats, ease)
                    L.Add(T)
                End If
            Next
            Return L
        End Function
        <Extension> Public Sub EditTreeColor(e As CallCustomMethod, room As Byte, location As Boolean, color As String, beats As Single, ease As EaseType)
            e.MethodName = $"room[{room}].{NameOf(EditTreeColor)}({location},{color},{beats},{ease})"
        End Sub
        <Extension> Public Sub MoveToPosition(e As Move, point As RDPoint)

        End Sub
    End Module
End Namespace