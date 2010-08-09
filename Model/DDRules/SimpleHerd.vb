﻿Public Class SimpleHerd
        Private ReferenceCow As SimpleCow
        Private TotalCows As Double

        Dim Total_DM_Eaten As BioMass = New BioMass()
        Dim Total_Pasutre_Eaten As BioMass = New BioMass()
        Dim Total_Supplement_Eaten As BioMass = New BioMass()

        Public ME_Demand As Double

#Region "Output Variables - Intake"
#Region "Output Variables - Intake - Dry Matter"
        Public ReadOnly Property DM_Eaten() As Double
                Get
                        Return Total_DM_Eaten.DM_Total
                End Get
        End Property

        Public ReadOnly Property DM_Eaten_Pasture() As Double
                Get
                        Return Total_Pasutre_Eaten.DM_Total
                End Get
        End Property

        Public ReadOnly Property DM_Eaten_Supplement() As Double
                Get
                        Return Total_Supplement_Eaten.DM_Total
                End Get
        End Property
#End Region
#Region "Output Variables - Intake - Energy"
        Public ReadOnly Property ME_Eaten() As Double
                Get
                        Return Total_DM_Eaten.getME_Total
                End Get
        End Property

        Public ReadOnly Property ME_Eaten_Pasture() As Double
                Get
                        Return Total_Pasutre_Eaten.getME_Total
                End Get
        End Property

        Public ReadOnly Property ME_Eaten_Supplement() As Double
                Get
                        Return Total_Supplement_Eaten.getME_Total
                End Get
        End Property
#End Region
#Region "Output Variables - Intake - Nitrogen"
        Public ReadOnly Property N_Eaten() As Double
                Get
                        Return Total_DM_Eaten.N_Total
                End Get
        End Property

        Public ReadOnly Property N_Eaten_Pasture() As Double
                Get
                        Return Total_Pasutre_Eaten.N_Total
                End Get
        End Property

        Public ReadOnly Property N_Eaten_Supplement() As Double
                Get
                        Return Total_Supplement_Eaten.N_Total
                End Get
        End Property
#End Region
#End Region
#Region "Output Variables - Outputs"
#Region "Output Variables - Outputs - Nitrogen/Carbon"
        Public ReadOnly Property N_to_feaces() As Double
                Get
                        Return ReferenceCow.N_to_feaces * TotalCows
                End Get
        End Property
        Public ReadOnly Property C_to_feaces() As Double
                Get
                        Return ReferenceCow.C_to_feaces * TotalCows
                End Get
        End Property
        Public ReadOnly Property N_to_urine() As Double
                Get
                        Return ReferenceCow.N_to_urine * TotalCows
                End Get
        End Property
        Public ReadOnly Property N_to_Milk() As Double
                Get
                        Return ReferenceCow.N_to_Milk * TotalCows
                End Get
        End Property
        Public ReadOnly Property N_to_BC() As Double
                Get
                        Return ReferenceCow.N_to_BC * TotalCows
                End Get
        End Property
        Public ReadOnly Property N_Balance() As Double
                Get
                        Return N_Eaten - N_Out
                End Get
        End Property
        Public ReadOnly Property N_Out() As Double
                Get
                        Return N_to_Milk + N_to_feaces + N_to_urine + N_to_BC
                End Get
        End Property
#End Region
#End Region

        Public Sub New(ByVal Head As Double, ByVal CalvingDate As Integer, ByVal Year As Integer, ByVal Month As Integer)
                ReferenceCow = New SimpleCow(Year, Month)
                ReferenceCow.CalvingDate = CalvingDate
                TotalCows = Head
        End Sub

        Public Function RemainingFeedDemand() As Double
                Return ReferenceCow.RemainingFeedDemand * TotalCows
        End Function

        Public Function TodaysEnergyRequirement() As Double
                Return ReferenceCow.TodaysEnergyRequirement * TotalCows
        End Function

        Public Function isDry() As Boolean
                Return ReferenceCow.isDry
        End Function

        Public Sub onPrepare(ByVal numCows As Double, ByVal Year As Integer, ByVal Month As Integer)
                ReferenceCow.OnPrepare(Year, Month)
                Total_DM_Eaten = New BioMass()
                Total_Pasutre_Eaten = New BioMass()
                Total_Supplement_Eaten = New BioMass()
                setCowNumbers(numCows)
        End Sub

        Public ReadOnly Property ME_Maintance()
                Get
                        Return ReferenceCow.ME_Maintance * TotalCows
                End Get
        End Property

        Public ReadOnly Property ME_Maintance_Cow()
                Get
                        Return ReferenceCow.ME_Maintance
                End Get
        End Property

        Public ReadOnly Property ME_WeightChange()
                Get
                        Return ReferenceCow.ME_WeightChange * TotalCows
                End Get
        End Property

        Public ReadOnly Property ME_WeightChange_Cow()
                Get
                        Return ReferenceCow.ME_WeightChange
                End Get
        End Property

        Public ReadOnly Property ME_Lactation()
                Get
                        Return ReferenceCow.ME_Lactation * TotalCows
                End Get
        End Property

        Public ReadOnly Property ME_Lactation_Cow()
                Get
                        Return ReferenceCow.ME_Lactation
                End Get
        End Property

        Public ReadOnly Property ME_Pregnancy()
                Get
                        Return ReferenceCow.ME_Pregnancy * TotalCows
                End Get
        End Property

        Public ReadOnly Property ME_Pregnancy_Cow()
                Get
                        Return ReferenceCow.ME_Pregnancy
                End Get
        End Property

        Public ReadOnly Property ME_Walking()
                Get
            Return ReferenceCow.ME_Walking * TotalCows
                End Get
        End Property

        Public ReadOnly Property ME_Walking_Cow()
                Get
                        Return ReferenceCow.ME_Walking
                End Get
        End Property
        Public ReadOnly Property ME_Total()
                Get
                        Return ReferenceCow.ME_Total * TotalCows
                End Get
        End Property

        Public Function ME_Total_Cow()
                Return ReferenceCow.ME_Total
        End Function

        Public Sub setCowNumbers(ByVal numCows As Double)
                TotalCows = numCows
                ME_Demand = ReferenceCow.TodaysEnergyRequirement * TotalCows
        End Sub

        Public Sub Feed(ByVal feed As BioMass, ByVal isPasture As Boolean)
                If (feed.DM_Total > 0) Then
                        If (isPasture) Then
                                Total_Pasutre_Eaten = Total_Pasutre_Eaten.Add(feed)
                        Else
                                Total_Supplement_Eaten = Total_Supplement_Eaten.Add(feed)
                        End If
                        Total_DM_Eaten = Total_DM_Eaten.Add(feed)
                        ReferenceCow.Feed(feed.Multiply(1 / TotalCows), isPasture)
                End If
        End Sub

        Public Function Graze(ByVal GrazingPaddock As LocalPaddockType, ByVal GrazingResidual As Double) As BioMass
                Dim dmRemoved As BioMass = GrazingPaddock.Graze(RemainingFeedDemand, GrazingResidual)
                If (dmRemoved.DM_Total > 0) Then
                        Feed(dmRemoved, True)
                End If
                Dim PH As Double = Total_DM_Eaten.getME_Total() 'PH == pasture harvested
                Return Total_DM_Eaten
        End Function

        Public Function isUnderFed() As Boolean
                Return RemainingFeedDemand() > 0
        End Function

        Public Function Live_Weight() As Double
                Return ReferenceCow.Live_Weight
        End Function

        Public Function LWt_Change() As Double
                Return ReferenceCow.Change_in_KgLWt_per_Day
        End Function

        Public Function BC() As Double
                Return ReferenceCow.CondistionScore
        End Function

        Public Function Month_Of_Pregnancy() As Integer
                Return ReferenceCow.Month_Of_Pregnancy
        End Function
        Public Function Month_Of_Lactation() As Integer
                Return ReferenceCow.Month_Of_Lactation
        End Function

        Public Function MS_per_Day() As Single
                Return ReferenceCow.MS_per_Day * TotalCows
        End Function

        Public Function MS_per_Day_Cow() As Single
                Return ReferenceCow.MS_per_Day
        End Function

        Public Function Number_Of_Cows() As Double
                Return TotalCows
        End Function

        Public Sub doNitrogenPartioning()
                ReferenceCow.doNitrogenPartioning()
        End Sub

        'Return all nutrients evenly to the paddocks in the list
        ' Todo: distribute nutrients by amount of drymatter removal (if grazed)
        Public Sub doNutrientReturns(ByVal myPaddocks As List(Of LocalPaddockType))
                Dim uN As Double = N_to_urine
                Dim dN As Double = N_to_feaces
                Dim dC As Double = C_to_feaces
                Dim totalMERemoved As Double = 0
                For Each pdk As LocalPaddockType In myPaddocks
                        totalMERemoved += pdk.ME_Eaten
                Next

                If totalMERemoved <= 0 Then 'if no grazing then distribute evenlly over all allocated paddocks
                        If (TotalCows > 0 And myPaddocks.Count > 0) Then
                                Dim delta As Double = 1 / myPaddocks.Count
                                Dim density As Double = delta * TotalCows
                                For Each pdk As LocalPaddockType In myPaddocks
                                        ReferenceCow.doNutrientReturns(pdk, uN * delta, dN * delta, dC * delta, density)
                                Next
                        End If
                Else 'if grazed then distribute over paddocks by amount of ME removed
                        If (TotalCows > 0 And myPaddocks.Count > 0) Then
                                For Each pdk As LocalPaddockType In myPaddocks
                                        Dim delta As Double = pdk.ME_Eaten / totalMERemoved
                                        Dim density As Double = delta * TotalCows
                                        ReferenceCow.doNutrientReturns(pdk, uN * delta, dN * delta, dC * delta, density)
                                Next
                        End If
                End If
        End Sub

        Public Function ME_Demand_Cow() As Double
                Return ReferenceCow.ME_Total
        End Function
        Public Function ME_Eaten_Cow() As Double
                Return ReferenceCow.ME_Eaten
        End Function
        Public Function ME_Eaten_Pasture_Cow() As Double
                Return ReferenceCow.ME_Eaten_Pasture
        End Function
        Public Function ME_Eaten_Supplement_Cow() As Double
                Return ReferenceCow.ME_Eaten_Supplement
        End Function
        Public Function DM_Eaten_Cow() As Double
                Return ReferenceCow.DM_Eaten
        End Function
        Public Function DM_Eaten_Pasture_Cow() As Double
                Return ReferenceCow.DM_Eaten_Pasture
        End Function
        Public Function DM_Eaten_Supplement_Cow() As Double
                Return ReferenceCow.DM_Eaten_Supplement
        End Function
        Public Function N_Eaten_Cow() As Double
                Return ReferenceCow.N_Eaten
        End Function
        Public Function N_Eaten_Pasture_Cow() As Double
                Return ReferenceCow.N_Eaten_Pasture
        End Function
        Public Function N_Eaten_Supplement_Cow() As Double
                Return ReferenceCow.N_Eaten_Supplement
        End Function
        Public Function N_to_milk_Cow() As Double
                Return ReferenceCow.N_to_Milk
        End Function
        Public Function N_to_BC_Cow() As Double
                Return ReferenceCow.N_to_BC
        End Function
        Public Function N_to_feaces_Cow() As Double
                Return ReferenceCow.N_to_feaces
        End Function
        Public Function N_to_urine_Cow() As Double
                Return ReferenceCow.N_to_urine
        End Function
End Class
