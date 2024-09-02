
SET IDENTITY_INSERT [dbo].[Config_Dashboard_DashboardPanels] ON 
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (6, NULL, NULL, N'SMS Summary', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>Service Management System</ContainerTitle>
  <Description>SMS Summary</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>0b5c5d12-842e-471f-9f28-9a71d36e01d1</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>AllOpenSMSTickets</DashboardTable>
      <LinkID>38ee6f0f-3189-4e39-8932-6c866555f76c</LinkID>
      <Title>All Open Tickets</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Open Tickets: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>AllOpenSMSTickets</DashboardTable>
      <LinkID>94fb4446-5082-4f34-b4c6-ec05bbfb8dc8</LinkID>
      <Title>High Priority Tickets</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>High Priority: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketPriorityLookup] = ''2-High'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>AllOpenSMSTickets</DashboardTable>
      <LinkID>3424bb02-b32b-47ee-9a6a-30b98c8080f8</LinkID>
      <Title>Waiting on Me</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Waiting on Me: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>f:MatchUser([TicketStageActionUsers], [Me])</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>AllOpenSMSTickets</DashboardTable>
      <LinkID>807f6c2d-cd9b-4f12-88b7-bf42b370c747</LinkID>
      <Title>Unassigned</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Unassigned: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter> [TicketStatus] = ''Pending Assignment'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>9e9366b2-2de5-48fc-bb0c-5ae8802c9be0</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, 1, NULL, NULL, 12, 200, 175, NULL, N'Accent2', N'Service Management System', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.140' AS DateTime), CAST(N'2018-01-19 12:08:39.140' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (7, NULL, NULL, N'Virtual PMO Summary', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>Virtual PMO</ContainerTitle>
  <Description>IT Governance</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>5c045d58-9cfa-4807-8f2d-9513a944506f</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>55aef6d3-b042-40bb-afb2-f735d78bd5b6</LinkID>
      <Title>Active Projects</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Active Projects: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketStatus] &lt;&gt; ''Closeout '' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>9fba701c-2cd3-468e-9583-c1d22eee7505</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, 1, NULL, NULL, 13, 200, 175, NULL, N'Accent2', N'Virtual PMO', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.140' AS DateTime), CAST(N'2018-01-19 12:08:39.140' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (8, NULL, NULL, N'RMM Resource Counts', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>RMM Resources</ContainerTitle>
  <Description>RMM Resource Counts</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>b718a655-0cdf-4326-8696-f069721b15cc</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#E7EAFE</BGColor>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>UserInformation</DashboardTable>
      <LinkID>03c17d3e-957b-4cfd-ac32-6638a252b4f4</LinkID>
      <Title>All IT Consultant</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>IT Consultants: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[IsConsultant] = ''1'' AND [IT] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>UserInformation</DashboardTable>
      <LinkID>f51a15b0-f27c-4811-bdfe-a8acd0e283bb</LinkID>
      <Title>IT Staff</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>IT Staff: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>([IsConsultant] = ''0'' OR [IsConsultant] is null) AND [IT] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>UserInformation</DashboardTable>
      <LinkID>27492830-bc81-4965-b9cc-e0092cbf851e</LinkID>
      <Title>Total IT Resources</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>2</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Total IT Resources:$exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[IT] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>true</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>021bab92-f364-475b-bb24-1ebe5a9d4c83</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, 1, NULL, NULL, 14, 200, 175, NULL, N'Accent1', N'RMM Resources', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.140' AS DateTime), CAST(N'2018-01-19 12:08:39.140' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (9, NULL, N'', N'$Date$: % Actuals', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>$Date$: % Actuals</ContainerTitle>
  <Description>$Date$: % Actuals</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>991082d3-e88d-44a2-b18d-8b15b1a8f9b7</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>a69d78c9-98a2-4bf2-a4c8-285907e9dc92</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Level1</Title>
      <SelectedField>WorkItemType</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Level2</Title>
      <SelectedField>WorkItem</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Level3</Title>
      <SelectedField>SubWorkItem</SelectedField>
      <Sequence>3</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Actuals</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Pie</ChartType>
      <ShowInPercentage>true</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText>{V} %</LabelText>
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ActualHour</FunctionExpression>
      <Palette>Pastel</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ResourceUsageSummaryMonthWise</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Previous Month</BasicDateFilterDefaultView>
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle>Auto</LabelStyle>
  <LabelText>$Exp$ %</LabelText>
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 8, 500, 400, N'', N'Accent1', N'$Date$: % Actuals', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.140' AS DateTime), CAST(N'2018-01-19 12:08:39.140' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (10, NULL, NULL, N'Projects Summary', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>Projects</ContainerTitle>
  <Description>Project Summary</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>385cbbab-eace-462e-9a72-1077738679b6</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMM-OpenTickets</DashboardTable>
      <LinkID>6ef00bfd-8019-431c-86a2-c69446a6e6fd</LinkID>
      <Title>Active Projects</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>true</UseAsPanel>
      <ExpressionFormat>Active Projects: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>NPR-OpenTickets</DashboardTable>
      <LinkID>c1a52138-0737-422e-aa46-842b1b88d924</LinkID>
      <Title>Pending Approval</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>2</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Pending Approval: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[StageStep] &gt; 1 AND [StageStep] &lt; 6 AND ([TicketOnHold] is null OR [TicketOnHold] = 0)</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>NPR-OpenTickets</DashboardTable>
      <LinkID>74c31224-7710-48d8-8264-aadab1aa71a7</LinkID>
      <Title>Ready for Action</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>3</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Ready for Action: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketStatus] = ''Ready for Action'' AND [TicketPMMIdLookup] = ''''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>b67c00e9-ad18-4487-99ae-07efe1a717c1</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, 1, NULL, NULL, 16, 200, 175, NULL, N'Accent1', N'Projects', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.140' AS DateTime), CAST(N'2018-01-19 12:08:39.140' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (11, NULL, N'', N'Previous Month Report', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>$Date$: Tickets Created</ContainerTitle>
  <Description>Previous Month Report</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>fa39a57a-de68-4517-be7f-09fad04b2c9b</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>8b94b4a4-a68b-4070-9104-93e4c74d16a6</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Modules</Title>
      <SelectedField>ModuleNameLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Total Tickets</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>[ID]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Current Quarter</BasicDateFilterDefaultView>
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 13, 300, 225, N'', N'Accent1', N'$Date$: Tickets Created', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.143' AS DateTime), CAST(N'2018-01-19 12:08:39.143' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (12, NULL, N'', N'Active Requests by Priority Summary', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Active Requests by Priority</ContainerTitle>
  <Description>Active Requests by Priority Summary</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>4a974f01-c18a-4f7e-b023-dec9917a5244</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>510c7ae0-70eb-410f-9919-c76ae78da408</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Modules</Title>
      <SelectedField>ModuleNameLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priorities</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>PriorityLookup</GroupByField>
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>[ID]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[GenericStatusLookup] = ''Unassigned'' OR [GenericStatusLookup] = ''Open''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <BGColor>#E7EAFE</BGColor>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 17, 300, 225, N'', N'Accent1', N'Active Requests by Priority', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.143' AS DateTime), CAST(N'2018-01-19 12:08:39.143' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (13, NULL, N'', N'Top 5 Problem Types (PRS)', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Top 5 Problem Types (PRS)</ContainerTitle>
  <Description>Top 5 Problem Types (PRS)</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>75a4793c-da0b-413b-b9a4-4d7c79afe62b</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>d7d5931b-c50b-45ad-a8f1-86ebf332afc7</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Category</Title>
      <SelectedField>Category</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>5</PickTopDataPoint>
      <DataPointOrder>Descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title># of Tickets</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Top</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>[ID]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Past 6 Months</BasicDateFilterDefaultView>
  <BasicFilter>[ModuleNameLookup] = ''PRS''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 18, 300, 225, N'', N'Accent1', N'Top 5 Problem Types (PRS)', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.143' AS DateTime), CAST(N'2018-01-19 12:08:39.143' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (14, NULL, N'', N'Top 5 Service Requests (TSR)', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Top 5 Service Requests (TSR)</ContainerTitle>
  <Description>Top 5 Service Requests (TSR)</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>e712af23-5314-43a5-a67e-76a27af081b9</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>2347b63c-b1a8-4b30-954b-0bbe28b7b718</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Initiator</Title>
      <SelectedField>Initiator</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Category</Title>
      <SelectedField>Category</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>descending</OrderBy>
      <EnableSorting>true</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Sub-Category</Title>
      <SelectedField>SubCategory</SelectedField>
      <Sequence>3</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>descending</OrderBy>
      <EnableSorting>true</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Request Type</Title>
      <SelectedField>RequestTypeLookup</SelectedField>
      <Sequence>4</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>descending</OrderBy>
      <EnableSorting>true</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title># of Tickets</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>[ID]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[ModuleNameLookup] = ''TSR''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 17, 600, 400, N'', N'Accent1', N'Top 5 Service Requests (TSR)', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.143' AS DateTime), CAST(N'2018-01-19 12:08:39.143' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (15, NULL, NULL, N'PRS Avg Days to Resolve by Priority', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>PRS Avg Days to Resolve by Priority</ContainerTitle>
  <Description>PRS Avg Days to Resolve by Priority</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>01740ba6-b547-41f6-a2c0-e61fc754d147</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>d392ced0-f224-4032-ac36-1b8c5aff6380</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Month</Title>
      <SelectedField>InitiatedDate</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Days</Title>
      <SelectedField>InitiatedDate</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>day</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priorities</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>avg</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>Daysdiff(ResolvedDate,InitiatedDate)</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>InitiatedDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Past 6 Months</BasicDateFilterDefaultView>
  <BasicFilter>ModuleNameLookup = ''PRS'' AND  [GenericStatusLookup] = ''Closed''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 21, 300, 225, NULL, N'Accent1', N'PRS Avg Days to Resolve by Priority', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.143' AS DateTime), CAST(N'2018-01-19 12:08:39.143' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (16, NULL, NULL, N'PRS Response Trends', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>PRS Response Trends</ContainerTitle>
  <Description>PRS Response Trends</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>15d9848c-9e84-41fa-848f-2ce08657b497</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>eb466367-603d-4fd2-9223-eda41865effa</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Month</Title>
      <SelectedField>InitiatedDate</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Days</Title>
      <SelectedField>InitiatedDate</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>day</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title># Of Tickets</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>GenericStatusLookup</GroupByField>
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>2</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>SPlineTensionPrcntg</Key>
          <Value xsi:type="xsd:string">75</Value>
        </DataItem>
        <DataItem>
          <Key>SPlineMarkerType</Key>
          <Value xsi:type="xsd:string">Circle</Value>
        </DataItem>
        <DataItem>
          <Key>SPlineMarkerSize</Key>
          <Value xsi:type="xsd:string">10</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title>Days to Respond</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>avg</Operator>
      <ExpressionFormula>GenericStatusLookup = ''Closed''</ExpressionFormula>
      <Order>3</Order>
      <ChartType>Spline</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Secondary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>Daysdiff(AssignedDate,InitiatedDate)</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>SPlineTensionPrcntg</Key>
          <Value xsi:type="xsd:string">75</Value>
        </DataItem>
        <DataItem>
          <Key>SPlineMarkerType</Key>
          <Value xsi:type="xsd:string">Circle</Value>
        </DataItem>
        <DataItem>
          <Key>SPlineMarkerSize</Key>
          <Value xsi:type="xsd:string">10</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title>Days to Resolve</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>avg</Operator>
      <ExpressionFormula>GenericStatusLookup = ''Closed''</ExpressionFormula>
      <Order>4</Order>
      <ChartType>Spline</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Secondary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>Daysdiff(ResolvedDate,InitiatedDate)</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>InitiatedDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>ModuleNameLookup = ''PRS''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 22, 300, 225, NULL, N'Accent1', N'PRS Response Trends', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.143' AS DateTime), CAST(N'2018-01-19 12:08:39.143' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (17, NULL, NULL, N'TSR Avg Days to Resolve by Priority', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>TSR Avg Days to Resolve by Priority</ContainerTitle>
  <Description>TSR Avg Days to Resolve by Priority</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>6077c742-1a97-4ef3-916c-05a5a4aa16dc</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#E7EAFE</BGColor>
  <Id />
  <ChartId>3d869fa1-9f35-468d-8a4b-e20316682406</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Month</Title>
      <SelectedField>InitiatedDate</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Days</Title>
      <SelectedField>InitiatedDate</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>day</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priorities</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>avg</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>Daysdiff(ResolvedDate,InitiatedDate)</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>InitiatedDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>ModuleNameLookup = ''TSR''  AND  [GenericStatusLookup] = ''Closed''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 23, 300, 300, NULL, N'Accent1', N'TSR Avg Days to Resolve by Priority', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.143' AS DateTime), CAST(N'2018-01-19 12:08:39.143' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (18, NULL, N'', N'TSR Response Trends', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>TSR Response Trends</ContainerTitle>
  <Description>TSR Response Trends</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>93ced789-3afa-4515-acb0-5651dd956ac2</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>26a20c48-aa02-46b3-9d18-7c45125ef56e</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Month</Title>
      <SelectedField>InitiatedDate</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
    <ChartDimension>
      <Title>Days</Title>
      <SelectedField>InitiatedDate</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>day</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title># of Requests</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>GenericStatusLookup</GroupByField>
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>[ID]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Days to Respond</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>avg</Operator>
      <ExpressionFormula>GenericStatusLookup = ''Closed''</ExpressionFormula>
      <Order>2</Order>
      <ChartType>Spline</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Secondary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>Daysdiff(AssignedDate,InitiatedDate)</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Days to Resolve</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>avg</Operator>
      <ExpressionFormula>GenericStatusLookup = ''Closed''</ExpressionFormula>
      <Order>3</Order>
      <ChartType>Spline</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Secondary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>Daysdiff(ResolvedDate,InitiatedDate)</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>InitiatedDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>ModuleNameLookup = ''TSR''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <BGColor>#FFFFFF</BGColor>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 23, 300, 300, N'', N'Accent1', N'TSR Response Trends', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.143' AS DateTime), CAST(N'2018-01-19 12:08:39.143' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (19, NULL, N'', N'% Allocation', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>$Date$: % Allocation</ContainerTitle>
  <Description>% Allocation</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>8d193704-523b-4587-8090-9203efb39dd7</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>421c525b-1e25-478d-8c1a-83b2a9213012</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Level1</Title>
      <SelectedField>WorkItemType</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Level2</Title>
      <SelectedField>WorkItem</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Level3</Title>
      <SelectedField>SubWorkItem</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Allocations</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>true</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>[AllocationHour]</FunctionExpression>
      <Palette>Pastel</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ResourceUsageSummaryMonthWise</FactTable>
  <BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Previous Month</BasicDateFilterDefaultView>
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <BGColor>#E7EAFE</BGColor>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 24, 500, 400, N'', N'Accent1', N'$Date$: % Allocation', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.147' AS DateTime), CAST(N'2018-01-19 12:08:39.147' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (20, NULL, NULL, N'Actual vs Allocated by Category', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>$Date$ Actual vs Allocated by Category</ContainerTitle>
  <Description>Actual vs Allocated by Category</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>90ee634e-7008-405f-9573-5532bd0bbd08</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>9a36e9e2-10ad-417f-9abd-c89f56186c27</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Category</Title>
      <SelectedField>WorkItemType</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Sub-Category</Title>
      <SelectedField>WorkItem</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Allocation Hours</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>true</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>[AllocationHour]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Actual Hours</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>true</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ActualHour</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ResourceUsageSummaryMonthWise</FactTable>
  <BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Previous Month</BasicDateFilterDefaultView>
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 26, 500, 400, NULL, N'Accent1', N'$Date$ Actual vs Allocated by Category', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.147' AS DateTime), CAST(N'2018-01-19 12:08:39.147' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (21, NULL, NULL, N'Actual vs Allocated by Manager', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>$Date$ Actual vs Allocated by Manager</ContainerTitle>
  <Description>Actual vs Allocated by Manager</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>5966f586-bf51-4021-9316-c8d7fab1eaa6</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>1457862b-f6b7-40d1-b2be-a24f06142c82</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Manager</Title>
      <SelectedField>ManagerName</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Resource</Title>
      <SelectedField>ResourceName</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Allocated Hours</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>true</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>[AllocationHour]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Actual Hours</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>true</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>[ActualHour]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ResourceUsageSummaryMonthWise</FactTable>
  <BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Previous Month</BasicDateFilterDefaultView>
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 27, 618, 460, NULL, N'Accent1', N'$Date$ Actual vs Allocated by Manager', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.147' AS DateTime), CAST(N'2018-01-19 12:08:39.147' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (22, NULL, NULL, N'Project Metrics', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>Project Metrics</ContainerTitle>
  <Description>Project Metrics</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>61366d42-6e0a-402d-8431-16983e995452</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>NPRRequest</DashboardTable>
      <LinkID>eed57b17-39a5-4748-98e6-45aa5a4d126c</LinkID>
      <Title>Pending Approval</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketStatus] = ''Manager Approval'' OR [TicketStatus] = ''IT PMO Assessment'' OR [TicketStatus] = ''IT Governance Review'' OR [TicketStatus] = ''IT Steering Committee Review''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>NPRRequest</DashboardTable>
      <LinkID>52fd81a5-3251-4579-b52a-f90e830c775b</LinkID>
      <Title>Ready to Start</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>TicketStatus = ''Approved'' AND TicketPMMIdLookup = ''''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>NPRRequest</DashboardTable>
      <LinkID>8d17c6da-ab96-4191-9335-c3b4e47efc6b</LinkID>
      <Title>Rejected</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>TicketStatus = ''Rejected''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>NPRRequest</DashboardTable>
      <LinkID>c8a1cdea-d83d-4fd9-bd31-e7bae089f6b2</LinkID>
      <Title>Current Projects</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>TicketPMMIdLookup &lt;&gt; '''' AND TicketStatus &lt;&gt; ''Closed'' AND TicketStatus &lt;&gt; ''On Hold''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>fe5e3566-7724-4af2-af24-ba775118d83a</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 28, 300, 225, NULL, N'Accent1', N'Project Metrics', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.147' AS DateTime), CAST(N'2018-01-19 12:08:39.147' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (23, NULL, NULL, N'Top 5 Projects', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>$Date$: Top 5 Projects</ContainerTitle>
  <Description>Top 5 Projects</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>0ed27b82-5632-4cc3-a357-b3db29482f21</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>ce362ef7-a908-4923-8aa6-685ea693e875</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Projects</Title>
      <SelectedField>Title</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>sum</Operator>
      <OperatorField>AllocationHour</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>20</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>barLablPosition</Key>
          <Value xsi:type="xsd:string">Center</Value>
        </DataItem>
        <DataItem>
          <Key>barLabelOrientation</Key>
          <Value xsi:type="xsd:string">Horizontal</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title>Allocation Hours</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Right</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>[AllocationHour]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ResourceUsageSummaryMonthWise</FactTable>
  <BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>WorkItemType = ''PMM''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 29, 480, 360, NULL, N'Accent1', N'$Date$: Top 5 Projects', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.147' AS DateTime), CAST(N'2018-01-19 12:08:39.147' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (24, NULL, NULL, N'Top 5 Production Support', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>$Date$: Top 5 Production Support</ContainerTitle>
  <Description>Top 5 Production Support</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>91776c1a-f186-4cc6-8c6b-be73aa88a7aa</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>8b505541-0eae-4865-8581-2f8eca924911</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Production Support</Title>
      <SelectedField>WorkItem</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>5</PickTopDataPoint>
      <DataPointOrder>Descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>sum</Operator>
      <OperatorField>AllocationHour</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>None</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Allocation Hours</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>[AllocationHour]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Actual Hours</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>[ActualHour]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ResourceUsageSummaryMonthWise</FactTable>
  <BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Previous Month</BasicDateFilterDefaultView>
  <BasicFilter>[WorkItemType] = ''Ticketing Support''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 30, 300, 225, NULL, N'Accent1', N'$Date$: Top 5 Production Support', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.167' AS DateTime), CAST(N'2018-01-19 12:08:39.167' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (25, NULL, NULL, N'Actuals (hours)', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>$Date$: Actuals (hours)</ContainerTitle>
  <Description>Actuals (hours)</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>4ff70328-62f4-4063-9745-436964610d2c</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>true</StartFromNewLine>
  <Id />
  <ChartId>a69d78c9-98a2-4bf2-a4c8-285907e9dc92</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Level1</Title>
      <SelectedField>WorkItemType</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Level2</Title>
      <SelectedField>WorkItem</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Level3</Title>
      <SelectedField>SubWorkItem</SelectedField>
      <Sequence>3</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Allocations</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>avg</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText>{V}</LabelText>
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ActualHour</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ResourceUsageSummaryMonthWise</FactTable>
  <BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Previous Month</BasicDateFilterDefaultView>
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle>Auto</LabelStyle>
  <LabelText>$Exp$</LabelText>
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 31, 300, 225, NULL, N'Accent1', N'$Date$: Actuals (hours)', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.167' AS DateTime), CAST(N'2018-01-19 12:08:39.167' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (26, NULL, N'none', N'Hi-Priority Open Tickets', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>3e8bca0e-4b71-4856-897f-0cff9dab28c9</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>3e8bca0e-4b71-4856-897f-0cff9dab28c9</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy>
      <GroupByInfo>
        <Column>
          <ID>12</ID>
          <FieldName>ModuleNameLookup</FieldName>
          <DisplayName>Module</DisplayName>
          <DataType>String</DataType>
          <TableName>DashboardSummary</TableName>
          <Function>none</Function>
          <Sequence>47</Sequence>
          <Selected>false</Selected>
          <Hidden>true</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
        </Column>
        <Num>1</Num>
      </GroupByInfo>
    </GroupBy>
    <OrderBy>
      <OrderByInfo>
        <Num>1</Num>
        <Column>
          <ID>29</ID>
          <FieldName>TicketId</FieldName>
          <DisplayName>Ticket Id</DisplayName>
          <DataType>String</DataType>
          <TableName>DashboardSummary</TableName>
          <Function>none</Function>
          <Sequence>1</Sequence>
          <Selected>true</Selected>
          <Hidden>false</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
          <Alignment>None</Alignment>
        </Column>
        <orderBy>ASC</orderBy>
      </OrderByInfo>
    </OrderBy>
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns>
          <ColumnInfo>
            <ID>41</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>Ticket Id</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>42</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>28</ID>
            <FieldName>TicketCreationDate</FieldName>
            <DisplayName>Created</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>43</ID>
            <FieldName>TicketStatus</FieldName>
            <DisplayName>Status</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>40</ID>
            <FieldName>TicketRequestTypeLookup</FieldName>
            <DisplayName>Request Type</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>5</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>34</ID>
            <FieldName>TicketOwner</FieldName>
            <DisplayName>Owner</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>6</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>36</ID>
            <FieldName>TicketPRP</FieldName>
            <DisplayName>PRP</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>7</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>35</ID>
            <FieldName>TicketPriorityLookup</FieldName>
            <DisplayName>Priority</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>8</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>15</ID>
            <FieldName>ModuleNameLookup</FieldName>
            <DisplayName>Module</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>43</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>0</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>0</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>DashboardSummary.TicketPriorityLookup</ColumnName>
        <DataType>String</DataType>
        <Operator>like</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>High</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
      <WhereInfo>
        <ID>2</ID>
        <RelationOpt>AND</RelationOpt>
        <ColumnName>DashboardSummary.GenericStatusLookup</ColumnName>
        <DataType>String</DataType>
        <Operator>NotEqual</Operator>
        <Valuetype>Constant</Valuetype>
        <Value>Closed</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals>
      <ColumnInfo>
        <ID>1</ID>
        <FieldName>TicketId</FieldName>
        <DisplayName>Ticket Id</DisplayName>
        <DataType>System.String</DataType>
        <TableName>DashboardSummary</TableName>
        <Function>Count</Function>
        <Sequence>1</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
    </Totals>
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns>
          <ColumnInfo>
            <ID>9</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>9</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text />
      <TextFontName>Times New Roman</TextFontName>
      <TextFontStyle>Bold</TextFontStyle>
      <TextFontSize>8pt</TextFontSize>
      <TextForeColor>000000</TextForeColor>
      <HideText>false</HideText>
      <Label />
      <LabelFontName>Times New Roman</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>8pt</LabelFontSize>
      <LabelForeColor>000000</LabelForeColor>
      <HideLabel>false</HideLabel>
      <ResultFontName>Times New Roman</ResultFontName>
      <ResultFontStyle>Bold</ResultFontStyle>
      <ResultFontSize>8pt</ResultFontSize>
      <ResultForeColor>000000</ResultForeColor>
      <TitlePosition>Top</TitlePosition>
      <FormatType>SimpleNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>0</Width>
        <Height>0</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
      <IconImage />
      <IconLocation>
        <X>0</X>
        <Y>0</Y>
      </IconLocation>
      <IconSize>
        <Width>0</Width>
        <Height>0</Height>
      </IconSize>
      <EnableEditUrl>false</EnableEditUrl>
      <BackgroundColor>ffffff</BackgroundColor>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>None</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <Header>Hi-Priority Open Tickets</Header>
      <Footer />
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <AdditionalInfo />
      <AdditionalFooterInfo>Aug-03-2017</AdditionalFooterInfo>
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>false</IsTransparent>
    </QueryFormats>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', N'', 2, N'', N'', 0, 0, 0, 0, 32, 0, 0, N'SMS', N'Accent1', N'Hi-Priority Open Tickets', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.170' AS DateTime), CAST(N'2018-01-19 12:08:39.170' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (27, NULL, N'Ticketing', N'UserInformation', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>c2166b13-4755-4cde-a456-9d387d750d1b</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#E7EAFE</BGColor>
  <Id />
  <QueryId>c2166b13-4755-4cde-a456-9d387d750d1b</QueryId>
  <QueryTable>User Information List</QueryTable>
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy>
      <OrderByInfo>
        <Num>1</Num>
        <Column>
          <ID>40</ID>
          <FieldName>Title</FieldName>
          <DisplayName>Name</DisplayName>
          <DataType>String</DataType>
          <TableName>User Information List</TableName>
          <Function>none</Function>
          <Sequence>1</Sequence>
          <Selected>true</Selected>
          <Hidden>false</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
          <Alignment>None</Alignment>
        </Column>
        <orderBy>ASC</orderBy>
      </OrderByInfo>
    </OrderBy>
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>User Information List</Name>
        <Columns>
          <ColumnInfo>
            <ID>0</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>User Information List</TableName>
            <Sequence>0</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>40</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Name</DisplayName>
            <DataType>String</DataType>
            <TableName>User Information List</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>12</ID>
            <FieldName>DepartmentLookup</FieldName>
            <DisplayName>Department</DisplayName>
            <DataType>String</DataType>
            <TableName>User Information List</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>30</ID>
            <FieldName>LocationLookup</FieldName>
            <DisplayName>Location</DisplayName>
            <DataType>String</DataType>
            <TableName>User Information List</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>31</ID>
            <FieldName>ManagerLookup</FieldName>
            <DisplayName>Manager</DisplayName>
            <DataType>User</DataType>
            <TableName>User Information List</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>43</ID>
            <FieldName>UserRoleLookup</FieldName>
            <DisplayName>User Role</DisplayName>
            <DataType>String</DataType>
            <TableName>User Information List</TableName>
            <Function>none</Function>
            <Sequence>8</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>25</ID>
            <FieldName>IT</FieldName>
            <DisplayName>IT</DisplayName>
            <DataType>Boolean</DataType>
            <TableName>User Information List</TableName>
            <Function>none</Function>
            <Sequence>5</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>23</ID>
            <FieldName>IsConsultant</FieldName>
            <DisplayName>IsConsultant</DisplayName>
            <DataType>Boolean</DataType>
            <TableName>User Information List</TableName>
            <Function>none</Function>
            <Sequence>6</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>24</ID>
            <FieldName>IsManager</FieldName>
            <DisplayName>IsManager</DisplayName>
            <DataType>Boolean</DataType>
            <TableName>User Information List</TableName>
            <Function>none</Function>
            <Sequence>7</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>User Information List.ContentType</ColumnName>
        <DataType>String</DataType>
        <Operator>Equal</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>Person</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals>
      <ColumnInfo>
        <ID>2</ID>
        <FieldName>Title</FieldName>
        <DisplayName>Name</DisplayName>
        <DataType>String</DataType>
        <TableName>User Information List</TableName>
        <Function>Count</Function>
        <Sequence>1</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
    </Totals>
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>User Information List</Name>
        <Columns>
          <ColumnInfo>
            <ID>0</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <TableName>User Information List</TableName>
            <Sequence>0</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </DrillDownTables>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, 1, NULL, NULL, NULL, 33, 0, 0, N'SMS', N'Accent1', N'User Information', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.170' AS DateTime), CAST(N'2018-01-19 12:08:39.170' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (28, NULL, N'Project Management', N'PMMProjectDetail', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>48a597b7-9145-4788-b5f8-21e9813af8e6</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>48a597b7-9145-4788-b5f8-21e9813af8e6</QueryId>
  <QueryTable>PMMProjects</QueryTable>
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy>
      <OrderByInfo>
        <Num>1</Num>
        <Column>
          <ID>97</ID>
          <FieldName>TicketId</FieldName>
          <DisplayName>Ticket ID</DisplayName>
          <DataType>String</DataType>
          <TableName>PMMProjects</TableName>
          <Function>none</Function>
          <Sequence>1</Sequence>
          <Selected>true</Selected>
          <Hidden>false</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
        </Column>
        <orderBy>ASC</orderBy>
      </OrderByInfo>
    </OrderBy>
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>PMMProjects</Name>
        <Columns>
          <ColumnInfo>
            <ID>45</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>PMMProjects</TableName>
            <Sequence>151</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>81</ID>
            <FieldName>TicketActualCompletionDate</FieldName>
            <DisplayName>Due Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>11</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>103</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>Ticket ID</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>115</ID>
            <FieldName>TicketPctComplete</FieldName>
            <DisplayName>% Complete</DisplayName>
            <DataType>Percent*100</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>12</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>116</ID>
            <FieldName>TicketPriorityLookup</FieldName>
            <DisplayName>Priority</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>5</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>119</ID>
            <FieldName>TicketProjectManager</FieldName>
            <DisplayName>Project Mgr(s)</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>10</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>136</ID>
            <FieldName>TicketStatus</FieldName>
            <DisplayName>Status</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>7</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>146</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>66</ID>
            <FieldName>ProjectRank</FieldName>
            <DisplayName>Rank</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>6</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>126</ID>
            <FieldName>TicketRequestTypeLookup</FieldName>
            <DisplayName>Project Type</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>8</ID>
            <FieldName>CompanyMultiLookup</FieldName>
            <DisplayName>Company</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>8</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>37</ID>
            <FieldName>DivisionMultiLookup</FieldName>
            <DisplayName>Division(s)</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>9</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>60</ID>
            <FieldName>ProjectCost</FieldName>
            <DisplayName>Spend To Date</DisplayName>
            <DataType>Currency</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>14</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>142</ID>
            <FieldName>TicketTotalCost</FieldName>
            <DisplayName>Budget Amount</DisplayName>
            <DataType>Currency</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>13</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>83</ID>
            <FieldName>TicketApprovedRFE</FieldName>
            <DisplayName>Project Code</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>150</ID>
            <FieldName>ProjectHealth</FieldName>
            <DisplayName>ProjectHealth</DisplayName>
            <DataType>none</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>15</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>PMMProjects.TicketClosed</ColumnName>
        <DataType>String</DataType>
        <Operator>NotEqual</Operator>
        <Valuetype>Constant</Valuetype>
        <Value>1</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals>
      <ColumnInfo>
        <ID>3</ID>
        <FieldName>TicketId</FieldName>
        <DisplayName>Ticket ID</DisplayName>
        <DataType>String</DataType>
        <TableName>PMMProjects</TableName>
        <Function>Count</Function>
        <Sequence>1</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
      <ColumnInfo>
        <ID>13</ID>
        <FieldName>ProjectCost</FieldName>
        <DisplayName>Spend To Date</DisplayName>
        <DataType>Currency</DataType>
        <TableName>PMMProjects</TableName>
        <Function>Sum</Function>
        <Sequence>14</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
      <ColumnInfo>
        <ID>14</ID>
        <FieldName>TicketTotalCost</FieldName>
        <DisplayName>Budget Amount</DisplayName>
        <DataType>Currency</DataType>
        <TableName>PMMProjects</TableName>
        <Function>Sum</Function>
        <Sequence>13</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
    </Totals>
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>PMMProjects</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text />
      <TextFontName>Times New Roman</TextFontName>
      <TextFontStyle>Bold</TextFontStyle>
      <TextFontSize>8pt</TextFontSize>
      <TextForeColor>000000</TextForeColor>
      <HideText>false</HideText>
      <Label />
      <LabelFontName>Times New Roman</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>8pt</LabelFontSize>
      <LabelForeColor>000000</LabelForeColor>
      <HideLabel>false</HideLabel>
      <ResultFontName>Times New Roman</ResultFontName>
      <ResultFontStyle>Bold</ResultFontStyle>
      <ResultFontSize>8pt</ResultFontSize>
      <ResultForeColor>000000</ResultForeColor>
      <TitlePosition>Top</TitlePosition>
      <FormatType>Table</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>0</Width>
        <Height>0</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
      <IconImage />
      <IconLocation>
        <X>0</X>
        <Y>0</Y>
      </IconLocation>
      <IconSize>
        <Width>0</Width>
        <Height>0</Height>
      </IconSize>
      <BackgroundColor>ffffff</BackgroundColor>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>None</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>false</IsTransparent>
    </QueryFormats>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, 1, NULL, NULL, NULL, 34, 0, 0, N'Project Listing', N'Accent1', N'PMM Project Detail', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.170' AS DateTime), CAST(N'2018-01-19 12:08:39.170' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (29, NULL, N'', N'Open Service Requests', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>Open Service Requests</ContainerTitle>
  <Description>Open Service Requests</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>9bc6230b-289f-4f9b-ad38-f172cd5e32e9</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>25a3b867-c284-435a-af78-0872786e4de9</LinkID>
      <Title>Critical</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''TSR'' AND [GenericStatusLookup] &lt;&gt; ''Closed'' AND [PriorityLookup] = ''1-Critical'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF0000</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>b56a1f24-eead-4365-b1e3-69a49cb6f98c</LinkID>
      <Title>High</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>2</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''TSR'' AND [GenericStatusLookup] &lt;&gt; ''Closed'' AND [PriorityLookup] = ''2-High'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF99CC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>2aaa1b96-3906-49e4-86f7-1864c24012ba</LinkID>
      <Title>Medium</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>3</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''TSR'' AND [GenericStatusLookup] &lt;&gt; ''Closed'' AND [PriorityLookup] = ''3-Medium'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FFC000</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>d2a2a1e4-bfb3-4936-ba5e-e6627bf30600</LinkID>
      <Title>Low</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>4</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''TSR'' AND [GenericStatusLookup] &lt;&gt; ''Closed'' AND [PriorityLookup] = ''4-Low''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#CCFFCC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>385e1b95-834f-461f-b4c9-6a5b7292c964</PanelID>
  <IconUrl />
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', N'', 0, N'Bold;#8pt;##000000', N'Bold;#8pt;##000000', 0, 0, 0, 0, 35, 200, 200, N'', N'Accent1', N'Open Service Requests', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.170' AS DateTime), CAST(N'2018-01-19 12:08:39.170' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (30, NULL, N'', N'Open Request Types', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>Open Request Types</ContainerTitle>
  <Description>Open Request Types</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>e0b82a8c-7543-4d79-a830-7f0c5cb396ed</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>40265088-9918-4b8c-9ad5-e375b304d454</LinkID>
      <Title>Issues</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField>TicketCreationDate</DateFilterStartField>
      <DateFilterDefaultView />
      <Filter> [ModuleNameLookup] = ''PRS'' AND [GenericStatusLookup] &lt;&gt; ''Closed''</Filter>
      <MaxLimit>50</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#953735</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#ffffff</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>ae5ae3d6-06ed-4c47-bcc7-7f6361a407bf</LinkID>
      <Title>Services</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField>TicketCreationDate</DateFilterStartField>
      <DateFilterDefaultView />
      <Filter> [ModuleNameLookup] = ''TSR'' AND [GenericStatusLookup] &lt;&gt; ''Closed''</Filter>
      <MaxLimit>50</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#0070C0</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#ffffff</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>abad2eca-d772-4ed1-af95-d607453112fe</LinkID>
      <Title>Changes</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter> [ModuleNameLookup] = ''ACR'' AND [GenericStatusLookup] &lt;&gt; ''Closed''</Filter>
      <MaxLimit>50</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#77933C</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#FFFFFF</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>ad67527e-64b9-4dab-8281-ad7fd892db3e</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', N'', 0, N'Bold;#8pt;##000000', N'Bold;#8pt;##000000', 0, 0, 0, 0, 36, 200, 200, N'', N'Accent1', N'Open Request Types', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.170' AS DateTime), CAST(N'2018-01-19 12:08:39.170' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (31, NULL, N'', N'Closed (Past month)', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>Closed (Past month)</ContainerTitle>
  <Description>Closed (Past month)</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>fcf026ef-0d69-40fa-bb0e-150caa579ea7</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>be90762b-28f2-4588-9926-8e331a3cd5af</LinkID>
      <Title>Issues</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField>TicketCreationDate</DateFilterStartField>
      <DateFilterDefaultView>Previous Month</DateFilterDefaultView>
      <Filter> [ModuleNameLookup] = ''PRS'' AND [TicketStatus] = ''Closed''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#953735</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#FFFFFF</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>c14a3039-13ca-4210-a1b3-be2cb9e00aa0</LinkID>
      <Title>Services</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField>TicketCreationDate</DateFilterStartField>
      <DateFilterDefaultView>Previous Month</DateFilterDefaultView>
      <Filter>[ModuleNameLookup] = ''TSR'' AND [TicketStatus] = ''Closed''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#0070C0</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#FFFFFF</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>ca9fea6d-5e20-4dce-9f08-b5a4644e3959</LinkID>
      <Title>Changes</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField>TicketCreationDate</DateFilterStartField>
      <DateFilterDefaultView>Previous Month</DateFilterDefaultView>
      <Filter>[ModuleNameLookup] = ''ACR'' AND [TicketStatus] = ''Closed''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#77933C</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#FFFFFF</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>adf6b3ae-4ac6-4e9f-861e-edeb849e27ca</PanelID>
  <IconUrl />
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', N'', 0, N'', N'', 0, 0, 0, 0, 36, 200, 100, N'', N'Accent1', N'Closed (Past month)', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.170' AS DateTime), CAST(N'2018-01-19 12:08:39.170' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (32, NULL, NULL, N'Service Requests (Past 6 Months)', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Service Requests (Past 6 mos)</ContainerTitle>
  <Description>Service Requests (Past 6 Months)</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>024b1581-b2ec-4362-af91-eb46d93428ed</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>bd50adb8-a37d-4ea9-b6f0-c76d5f7b391b</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Creation Date</Title>
      <SelectedField>TicketCreationDate</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>6</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>TicketCreationDate</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priority</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>ModuleNameLookup = ''PRS'' OR ModuleNameLookup = ''TSR''</BasicFilter>
  <HideDateFilterDropdown>true</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 38, 300, 300, NULL, N'Accent1', N'Service Requests (Past 6 mos)', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.170' AS DateTime), CAST(N'2018-01-19 12:08:39.170' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (33, NULL, N'Project Management', N'Task List', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>11b3e564-d3f4-40ca-ac5e-a04d931b0379</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>11b3e564-d3f4-40ca-ac5e-a04d931b0379</QueryId>
  <QueryTable>TSKProjects</QueryTable>
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy>
      <OrderByInfo>
        <Num>1</Num>
        <Column>
          <ID>39</ID>
          <FieldName>TicketId</FieldName>
          <DisplayName>Ticket ID</DisplayName>
          <DataType>String</DataType>
          <TableName>TSKProjects</TableName>
          <Function>none</Function>
          <Sequence>1</Sequence>
          <Selected>true</Selected>
          <Hidden>false</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
        </Column>
        <orderBy>ASC</orderBy>
      </OrderByInfo>
    </OrderBy>
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>TSKProjects</Name>
        <Columns>
          <ColumnInfo>
            <ID>16</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>TSKProjects</TableName>
            <Sequence>25</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>37</ID>
            <FieldName>TicketActualCompletionDate</FieldName>
            <DisplayName>Due Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>TSKProjects</TableName>
            <Function>none</Function>
            <Sequence>7</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>46</ID>
            <FieldName>TicketCreationDate</FieldName>
            <DisplayName>Ticket Creation Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>TSKProjects</TableName>
            <Function>none</Function>
            <Sequence>6</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>50</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>Ticket ID</DisplayName>
            <DataType>String</DataType>
            <TableName>TSKProjects</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>62</ID>
            <FieldName>TicketPctComplete</FieldName>
            <DisplayName>% Complete</DisplayName>
            <DataType>Percent</DataType>
            <TableName>TSKProjects</TableName>
            <Function>none</Function>
            <Sequence>8</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>63</ID>
            <FieldName>TicketPriorityLookup</FieldName>
            <DisplayName>Priority</DisplayName>
            <DataType>String</DataType>
            <TableName>TSKProjects</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>64</ID>
            <FieldName>TicketProjectManager</FieldName>
            <DisplayName>Project Manager</DisplayName>
            <DataType>String</DataType>
            <TableName>TSKProjects</TableName>
            <Function>none</Function>
            <Sequence>5</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>78</ID>
            <FieldName>TicketStatus</FieldName>
            <DisplayName>Status</DisplayName>
            <DataType>String</DataType>
            <TableName>TSKProjects</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>88</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>TSKProjects</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>89</ID>
            <FieldName>UGITDaysToComplete</FieldName>
            <DisplayName>DaysToComplete</DisplayName>
            <DataType>Integer</DataType>
            <TableName>TSKProjects</TableName>
            <Function>none</Function>
            <Sequence>9</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>TSKProjects.TicketStatus</ColumnName>
        <DataType>String</DataType>
        <Operator>NotEqual</Operator>
        <Valuetype>Constant</Valuetype>
        <Value>Closed</Value>
        <ParameterName />
      </WhereInfo>
    </WhereClauses>
    <Totals>
      <ColumnInfo>
        <ID>4</ID>
        <FieldName>TicketId</FieldName>
        <DisplayName>Ticket ID</DisplayName>
        <DataType>String</DataType>
        <TableName>TSKProjects</TableName>
        <Function>Count</Function>
        <Sequence>1</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
    </Totals>
    <DrillDownTables />
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 39, 0, 0, N'Task List', N'Accent1', N'Task List', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.170' AS DateTime), CAST(N'2018-01-19 12:08:39.170' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (34, NULL, NULL, N'New Project Requests', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>New Project Requests</ContainerTitle>
  <Description>New Project Requests</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>b8587dfc-a6a8-43b6-99ac-4368012b9d46</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>NPRRequest</DashboardTable>
      <LinkID>929175da-d1c6-434f-9066-ea946e901bf5</LinkID>
      <Title>Pending Review</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketStatus] = ''IT Governance Review'' OR [TicketStatus] = ''IT Steering Committee Review''</Filter>
      <MaxLimit>50</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#00FFFF</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>NPRRequest</DashboardTable>
      <LinkID>d711b1bc-1109-43fb-b391-f57d2b2a115e</LinkID>
      <Title>Pending Approval</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketStatus] = ''IT Governance Review''</Filter>
      <MaxLimit>50</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FFFF99</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>NPRRequest</DashboardTable>
      <LinkID>9b57b39f-42fe-46b2-aa49-09e277d39929</LinkID>
      <Title>Pending Start</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketStatus] = ''Approved'' AND [TicketPMMIdLookup] = ''''</Filter>
      <MaxLimit>50</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#00FF00</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>49678623-d510-4038-86a5-bb442582d19f</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 40, 200, 100, NULL, N'Accent1', N'New Project Requests', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.170' AS DateTime), CAST(N'2018-01-19 12:08:39.170' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (35, NULL, NULL, N'Project Tasks', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>Project Tasks</ContainerTitle>
  <Description>Project Tasks</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>c6377ef6-0ac0-4473-92c7-e54ea492e4f4</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>Task-Open</DashboardTable>
      <LinkID>068703bb-6550-4902-b5fd-97e00b7394d6</LinkID>
      <Title>Open Tasks</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#99CCFF</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>9cd35f64-89bc-41a9-b3d3-23c25df44d23</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 41, 200, 200, NULL, N'Accent1', N'Project Tasks', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.173' AS DateTime), CAST(N'2018-01-19 12:08:39.173' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (36, NULL, NULL, N'Project Status', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>Project Status</ContainerTitle>
  <Description>Project Status</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>c1c29f25-f9db-4aff-ba79-3f4b573ecee9</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>86a61d42-3133-453f-8eb8-8770f3e00b9e</LinkID>
      <Title>On Track</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter> IsNull(Convert([TicketProjectScore], ''System.Double''), 0) &gt; 90</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#00FF00</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>958ef5cb-d12c-4273-aec2-c5062e59d460</LinkID>
      <Title>Minor Issues</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter> IsNull(Convert([TicketProjectScore], ''System.Double''), 0) &gt;= 80</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FFFF00</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>199fa9d5-d967-4f3a-9a71-5be18790fa10</LinkID>
      <Title>Major Issues</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter> IsNull(Convert([TicketProjectScore], ''System.Double''), 0) &lt; 80</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF99CC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>01251e3c-27a1-476f-84eb-48b47580fb00</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 42, 200, 100, NULL, N'Accent1', N'Project Status', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.173' AS DateTime), CAST(N'2018-01-19 12:08:39.173' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (37, NULL, NULL, N'Resource Management', N'', N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>Resource Management</ContainerTitle>
  <Description>Resource Management</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>b56189f7-6c1d-4699-8e05-ca1b7c88959f</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>UserInformation</DashboardTable>
      <LinkID>67970bc7-933f-49fe-9612-9a7cc8e736e9</LinkID>
      <Title>Employees</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[IsConsultant] = ''0'' OR [IsConsultant] is null</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#CCFFFF</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>UserInformation</DashboardTable>
      <LinkID>790cede8-2445-4bd3-bf2d-4f25080c6218</LinkID>
      <Title>Consultants</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[IsConsultant] = ''1''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#CCFFCC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>5072819c-746c-410b-bb86-e84a0092d1e8</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 43, 200, 100, NULL, N'Accent1', N'Resource Management', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.173' AS DateTime), CAST(N'2018-01-19 12:08:39.173' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (38, NULL, NULL, N'Top 3 Resource-centric Projects', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Top 3 Resource-centric Projects</ContainerTitle>
  <Description>Top 3 Resource-centric Projects</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>79b74ac0-0271-4e05-b243-64ab47e4d292</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>240c8f5e-ce7e-4645-beb9-29f696888595</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Top Project</Title>
      <SelectedField>WorkItem</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>3</PickTopDataPoint>
      <DataPointOrder>Descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>sum</Operator>
      <OperatorField>ActualHour</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Actual Hours</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ActualHour</FunctionExpression>
      <Palette>Light</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ResourceUsageSummaryMonthWise</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[WorkItemType] = ''PMM''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 44, 200, 100, NULL, N'Accent1', N'Top 3 Resource-centric Projects', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.173' AS DateTime), CAST(N'2018-01-19 12:08:39.173' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (39, NULL, NULL, N'Resource Hours', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Resource Hours</ContainerTitle>
  <Description>Resource Hours</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>fde3c385-687c-46d2-8fe7-6673fee2371f</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>94101372-cb12-4436-b960-fbed58cb1f7d</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Resource Hours</Title>
      <SelectedField>WorkItemType</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>10</PickTopDataPoint>
      <DataPointOrder>Descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>WorkItemType</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Hours</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ActualHour</FunctionExpression>
      <Palette>Light</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ResourceUsageSummaryMonthWise</FactTable>
  <BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Previous Month</BasicDateFilterDefaultView>
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 45, 200, 100, NULL, N'Accent1', N'Resource Hours', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.173' AS DateTime), CAST(N'2018-01-19 12:08:39.173' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (40, NULL, NULL, N'Resource Utilization', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Resource Utilization</ContainerTitle>
  <Description>Resource Utilization</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>7c316a4f-799c-4f11-8a52-d84bc28416a5</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>fa0929de-4f1a-4642-9a01-4bb388e2ff92</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Types</Title>
      <SelectedField>WorkItemType</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>sideBySideFullStackedBarValAsPertge</Key>
          <Value xsi:type="xsd:string">True</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title>Allocation</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>true</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText>{V}%</LabelText>
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>AllocationHour</FunctionExpression>
      <Palette>BrightPastel</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ResourceUsageSummaryMonthWise</FactTable>
  <BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle>Auto</LabelStyle>
  <LabelText>$Exp$%</LabelText>
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 46, 500, 400, NULL, N'Accent1', N'Resource Utilization', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.173' AS DateTime), CAST(N'2018-01-19 12:08:39.173' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (41, NULL, NULL, N'Issues (PRS)', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>Issues (PRS)</ContainerTitle>
  <Description>Issues (PRS)</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>df001490-1327-4091-85ac-7a7bef2182ff</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>73592a5c-2603-478a-945b-ec101ac0485b</LinkID>
      <Title>Low</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''PRS'' AND [TicketPriorityLookup] = ''3-Low''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#CCFFCC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>true</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>26cbaf51-6eb1-4f27-9640-ce04c6e292a2</LinkID>
      <Title>Medium</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>2</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''PRS'' AND [TicketPriorityLookup] = ''1-Medium''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FFFF99</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>851bfa9d-979e-4496-b180-b17a0dd1e743</LinkID>
      <Title>High</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>3</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter> [ModuleNameLookup] = ''PRS'' AND [TicketPriorityLookup] = ''1-High''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF99CC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>f4f5246e-b231-47ff-b00e-31d83050ec41</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 47, 200, 250, NULL, N'Accent1', N'Issues (PRS)', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.173' AS DateTime), CAST(N'2018-01-19 12:08:39.173' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (42, NULL, NULL, N'Top 3 Issue Categories', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Top 3 Issue Categories</ContainerTitle>
  <Description>Top 3 Issue Categories</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>11fc7a38-26c9-4d25-a694-08b23e4a088e</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>53a1dc18-1ab9-4377-b2cd-c5611c4db494</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Request Types</Title>
      <SelectedField>TicketRequestTypeLookup</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>3</PickTopDataPoint>
      <DataPointOrder>Descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator />
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Request types</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator />
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>TicketActualHours</FunctionExpression>
      <Palette>Light</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[ModuleNameLookup] = ''PRS''</BasicFilter>
  <HideDateFilterDropdown>true</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle>Auto</LabelStyle>
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 48, 200, 250, NULL, N'Accent1', N'Top 3 Issue Categories', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.173' AS DateTime), CAST(N'2018-01-19 12:08:39.173' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (43, NULL, N'', N'Test', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>2ef06894-8d10-4ab5-ac79-06810b0b1e48</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <QueryId>00000000-0000-0000-0000-000000000000</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy>
      <OrderByInfo>
        <Num>1</Num>
        <Column>
          <ID>41</ID>
          <FieldName>TicketId</FieldName>
          <DisplayName>TicketId</DisplayName>
          <DataType>String</DataType>
          <TableName>DashboardSummary</TableName>
          <Function>none</Function>
          <Sequence>8</Sequence>
          <Selected>true</Selected>
          <Hidden>false</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
          <Alignment>None</Alignment>
        </Column>
        <orderBy>ASC</orderBy>
      </OrderByInfo>
    </OrderBy>
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns>
          <ColumnInfo>
            <ID>41</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>TicketId</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>8</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>8</ID>
            <FieldName>CreationDate</FieldName>
            <DisplayName>Created</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>19</ID>
            <FieldName>Owner</FieldName>
            <DisplayName>Owner</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>20</ID>
            <FieldName>PriorityLookup</FieldName>
            <DisplayName>Priority Lookup</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>21</ID>
            <FieldName>PRP</FieldName>
            <DisplayName>PRP</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>30</ID>
            <FieldName>RequestTypeLookup</FieldName>
            <DisplayName>Request Type Lookup</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>5</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>39</ID>
            <FieldName>Status</FieldName>
            <DisplayName>Status</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>6</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>42</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>7</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>15</ID>
            <FieldName>ModuleNameLookup</FieldName>
            <DisplayName>Module</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>43</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>0</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>0</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>DashboardSummary.PriorityLookup</ColumnName>
        <DataType>String</DataType>
        <Operator>like</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>High</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
      <WhereInfo>
        <ID>2</ID>
        <RelationOpt>AND</RelationOpt>
        <ColumnName>DashboardSummary.GenericStatusLookup</ColumnName>
        <DataType>String</DataType>
        <Operator>NotEqual</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>Closed</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals>
      <ColumnInfo>
        <ID>7</ID>
        <FieldName>TicketId</FieldName>
        <DisplayName>TicketId</DisplayName>
        <DataType>String</DataType>
        <TableName>DashboardSummary</TableName>
        <Function>Count</Function>
        <Sequence>8</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
    </Totals>
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
  </QueryInfo>
</DashboardQuery>', N'', 2, N'', N'', 0, 0, 0, 0, 35, 0, 0, N'none', N'Accent1', N'Test', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.177' AS DateTime), CAST(N'2018-01-19 12:08:39.177' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (44, NULL, NULL, N'PRS Issues', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>All PRS Issues</ContainerTitle>
  <Description>PRS Issues</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>95070638-24c4-46d4-8b7e-0b69500fb01c</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>7e7cb183-0c9f-435b-9046-4dcaabec809a</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Past Months</Title>
      <SelectedField>TicketCreationDate</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>3</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>issues</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[ModuleNameLookup] = ''PRS''</BasicFilter>
  <HideDateFilterDropdown>true</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 49, 300, 300, NULL, N'Accent1', N'All PRS Issues', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.177' AS DateTime), CAST(N'2018-01-19 12:08:39.177' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (45, NULL, NULL, N'Services (TSR)', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>Services (TSR)</ContainerTitle>
  <Description>Services (TSR)</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>be9f8e67-b6e6-43cc-acfa-a604994eb6c2</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>20706e6e-83fd-4a7b-aa83-a382fac0da2b</LinkID>
      <Title>Low</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''TSR'' AND [TicketPriorityLookup] = ''4-Low'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#CCFFCC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>5358bdf6-d92c-491b-b0fa-2dda7d6345a9</LinkID>
      <Title>Medium</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''TSR'' AND [TicketPriorityLookup] = ''3-Medium'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FFFF99</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>ec4ec8c5-7900-4842-bee1-250f2c476691</LinkID>
      <Title>High</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''TSR'' AND [TicketPriorityLookup] = ''2-High'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF99CC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>7dbaa3d9-aa07-489f-a578-470b88c77077</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 50, 200, 250, NULL, N'Accent1', N'Services (TSR)', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.177' AS DateTime), CAST(N'2018-01-19 12:08:39.177' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (46, NULL, NULL, N'Top 3 Service Categories', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Top 3 Service Categories</ContainerTitle>
  <Description>Top 3 Service Categories</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>62d0ed61-5369-40d0-aa13-a2e415df7814</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>05e4b008-c741-477d-b790-6191aad49344</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Category</Title>
      <SelectedField>Category</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>3</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>1</OrderByExpression>
      <OrderBy>descending</OrderBy>
      <EnableSorting>true</EnableSorting>
      <Operator />
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>barLablPosition</Key>
          <Value xsi:type="xsd:string">Center</Value>
        </DataItem>
        <DataItem>
          <Key>barLabelOrientation</Key>
          <Value xsi:type="xsd:string">Horizontal</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title>Category Count</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>Light</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[ModuleNameLookup] = ''TSR''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 51, 600, 400, NULL, N'Accent1', N'Top 3 Service Categories', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.177' AS DateTime), CAST(N'2018-01-19 12:08:39.177' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (47, NULL, NULL, N'TSR Requests', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>TSR Requests</ContainerTitle>
  <Description>TSR Requests</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>818e5064-e47d-408e-b362-333c4e306c38</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#FFFFFF</BGColor>
  <Id />
  <ChartId>0542da9d-bc03-4f98-9636-9a52fe72a6d7</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>create Date</Title>
      <SelectedField>TicketCreationDate</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>3</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priorities</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[ModuleNameLookup] = ''TSR''</BasicFilter>
  <HideDateFilterDropdown>true</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 52, 300, 300, NULL, N'Accent1', N'TSR Requests', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.177' AS DateTime), CAST(N'2018-01-19 12:08:39.177' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (48, NULL, N'', N'Change Requests (ACR)', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>Change Requests (ACR)</ContainerTitle>
  <Description>Change Requests (ACR)</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>95374563-4bf3-4433-afe6-08040c0da85a</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>7a3cda26-6b16-4c5b-a735-8c1cddc5b463</LinkID>
      <Title>Low</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''ACR'' AND [PriorityLookup] = ''4-Low'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#CCFFCC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>567d5bc5-e073-4907-af24-74b88d23a56e</LinkID>
      <Title>Medium</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''ACR'' AND [PriorityLookup] = ''3-Medium'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FFFF99</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>dc7750a6-60fb-4fc3-868d-a3ee7d85938a</LinkID>
      <Title>High</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''ACR'' AND [PriorityLookup] = ''2-High'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF99CC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>ea2b5c33-c416-4323-8c0b-861b27a15db2</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', N'', 0, N'', N'', 0, 0, 0, 0, 53, 200, 250, N'', N'Accent1', N'Change Requests (ACR)', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.177' AS DateTime), CAST(N'2018-01-19 12:08:39.177' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (49, NULL, NULL, N'Top 3 Change Categories', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Top 3 Change Categories</ContainerTitle>
  <Description>Top 3 Change Categories</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>523e399e-fc02-49ba-b949-b2daab70dcda</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>af5128d9-243b-443c-bc61-6f96ecb0a8b5</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Request Types</Title>
      <SelectedField>TicketRequestTypeLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>3</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>1</DataPointExpression>
      <OrderByExpression>1</OrderByExpression>
      <OrderBy>descending</OrderBy>
      <EnableSorting>true</EnableSorting>
      <Operator />
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>barLablPosition</Key>
          <Value xsi:type="xsd:string">Center</Value>
        </DataItem>
        <DataItem>
          <Key>barLabelOrientation</Key>
          <Value xsi:type="xsd:string">Horizontal</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title>Request types</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>Light</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[ModuleNameLookup] = ''ACR''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle>Auto</LabelStyle>
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 54, 600, 400, NULL, N'Accent1', N'Top 3 Change Categories', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.177' AS DateTime), CAST(N'2018-01-19 12:08:39.177' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (50, NULL, NULL, N'Application Changes', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Changes</ContainerTitle>
  <Description>Application Changes</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>5beddcdb-2753-45e3-b58b-217f2b13496a</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>47f91626-54ff-4435-9a0d-503615b6f12a</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Creation Date</Title>
      <SelectedField>TicketCreationDate</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>3</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priorities</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[ModuleNameLookup] = ''ACR''</BasicFilter>
  <HideDateFilterDropdown>true</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 55, 300, 300, NULL, N'Accent1', N'Change', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.180' AS DateTime), CAST(N'2018-01-19 12:08:39.180' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (51, NULL, NULL, N'Deployment Requests (DRQ)', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>Deployment Requests (DRQ)</ContainerTitle>
  <Description>Deployment Requests (DRQ)</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>b694bf41-5cf2-4c80-bdc6-f626bbe886cb</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>031b0dfe-ab2e-4fe1-ac03-cd45762fba99</LinkID>
      <Title>Low</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''DRQ'' AND [TicketPriorityLookup] = ''4-Low'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#CCFFCC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>ef20d25f-14df-4c6d-a7e0-104e46a3e726</LinkID>
      <Title>Medium</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''DRQ'' AND [TicketPriorityLookup] = ''3-Medium'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FFFF99</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>DashboardSummary</DashboardTable>
      <LinkID>6f0b7249-9e58-4ff5-9311-d36b89f1ccbb</LinkID>
      <Title>High</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[ModuleNameLookup] = ''DRQ'' AND [TicketPriorityLookup] = ''2-High'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF99CC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>876a33f9-3c32-4b09-bd87-14df46ea8ea7</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 56, 200, 250, NULL, N'Accent1', N'Deployment Requests (DRQ)', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.180' AS DateTime), CAST(N'2018-01-19 12:08:39.180' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (52, NULL, NULL, N'Last 3 Deployments', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Last 3 Deployments</ContainerTitle>
  <Description>Last 3 Deployments</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>26410716-28a5-46ba-87a1-cab6dff2710f</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>f71cbcc5-ade7-49f0-bd54-5dfba272176b</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Top 3</Title>
      <SelectedField>Title</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>3</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>descending</OrderBy>
      <EnableSorting>true</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>barLablPosition</Key>
          <Value xsi:type="xsd:string">Center</Value>
        </DataItem>
        <DataItem>
          <Key>barLabelOrientation</Key>
          <Value xsi:type="xsd:string">Horizontal</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title>Top 3</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator />
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>Light</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[ModuleNameLookup] = ''DRQ''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Nature Colors</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 57, 480, 360, NULL, N'Accent1', N'Last 3 Deployments', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.183' AS DateTime), CAST(N'2018-01-19 12:08:39.183' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (53, NULL, NULL, N'Deployments', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Deployments</ContainerTitle>
  <Description>Deployments</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>df75b741-53f6-444b-af89-ac3bcdf3c405</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>6d1d4e9d-0530-405d-8ccc-206fd3485f55</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Request Types</Title>
      <SelectedField>TicketCreationDate</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>6</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priorities</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[ModuleNameLookup] = ''DRQ''</BasicFilter>
  <HideDateFilterDropdown>true</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 58, 300, 300, NULL, N'Accent1', N'Deployments', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.183' AS DateTime), CAST(N'2018-01-19 12:08:39.183' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (54, NULL, NULL, N'Top 3 Requesting Department For NPR', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Top 3 Requesting Department For NPR</ContainerTitle>
  <Description>Top 3 Requesting Department For NPR</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>806839cb-20db-4f7e-b805-233e0071494f</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>92d281b4-4789-431f-b96e-4f5ac9a4eed5</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Top 3 Department</Title>
      <SelectedField>TicketBeneficiaries</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>3</PickTopDataPoint>
      <DataPointOrder>Descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator />
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>NPRRequest</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Top 3 Department</Title>
      <FactTable>NPRRequest</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>EstimatedHours</FunctionExpression>
      <Palette>Light</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>NPRRequest</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 60, 200, 100, NULL, N'Accent1', N'Top 3 Requesting Department For NPR', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.183' AS DateTime), CAST(N'2018-01-19 12:08:39.183' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (55, NULL, NULL, N'NPR Trend (Past 6 mos)', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>NPR Trend (Past 6 mos)</ContainerTitle>
  <Description>NPR Trend (Past 6 Month)</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>0a16dd9a-825c-473e-8f00-0c3f257c1272</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>943a66dc-1f05-467f-8579-8180d61603b8</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>NPR Past 6 Month</Title>
      <SelectedField>Created</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator />
      <OperatorField>Created</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>NPRRequest</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>NPR Trend past 6 month</Title>
      <FactTable>NPRRequest</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Spline</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>NPRRequest</FactTable>
  <BasicDateFitlerStartField>Created</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Past 6 Months</BasicDateFilterDefaultView>
  <BasicFilter />
  <HideDateFilterDropdown>true</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 61, 200, 250, NULL, N'Accent1', N'NPR Trend (Past 6 mos)', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.183' AS DateTime), CAST(N'2018-01-19 12:08:39.183' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (56, NULL, NULL, N'Task Lists Date Wise', NULL, N'<?xml version="1.0" encoding="UTF-8"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
   <type>Panel</type>
   <ContainerTitle>Task Lists Date Wise</ContainerTitle>
   <Description>TSK Trend (Past 6 Month)</Description>
   <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
   <Order>0</Order>
   <DashboardID>dbb82a08-2dc0-4c11-909e-e390c1ddb878</DashboardID>
   <HideZoomView>false</HideZoomView>
   <HideTableView>false</HideTableView>
   <HidewDownloadView>false</HidewDownloadView>
   <StartFromNewLine>false</StartFromNewLine>
   <Expressions>
      <DashboardPanelLink>
         <PanelModuleType>All</PanelModuleType>
         <ExpressionID>0</ExpressionID>
         <FormulaId>0</FormulaId>
         <ViewType>0</ViewType>
         <ModuleName />
         <DashboardTable>TSKProjects</DashboardTable>
         <LinkID>0b1a4cd4-dbff-4369-aa6a-d4b3005561e3</LinkID>
         <Title>Last 30 Days</Title>
         <LinkUrl />
         <DefaultLink>true</DefaultLink>
         <ScreenView>1</ScreenView>
         <IsHide>false</IsHide>
         <Order>0</Order>
         <UseAsPanel>false</UseAsPanel>
         <ExpressionFormat>$exp$</ExpressionFormat>
         <HideTitle>false</HideTitle>
         <DateFilterStartField>Created</DateFilterStartField>
         <DateFilterDefaultView>Last 30 Days</DateFilterDefaultView>
         <Filter />
         <MaxLimit>100</MaxLimit>
         <DecimalPoint>0</DecimalPoint>
         <ShowBar>true</ShowBar>
         <AggragateFun>Count</AggragateFun>
         <AggragateOf>[ID]</AggragateOf>
         <BarDefaultColor>#FFBB4F</BarDefaultColor>
         <IsPct>false</IsPct>
         <Conditions />
         <FontColor>#000000</FontColor>
         <BarUnit />
         <StopLinkDetail>false</StopLinkDetail>
      </DashboardPanelLink>
      <DashboardPanelLink>
         <PanelModuleType>All</PanelModuleType>
         <ExpressionID>0</ExpressionID>
         <FormulaId>0</FormulaId>
         <ViewType>0</ViewType>
         <ModuleName />
         <DashboardTable>TSKProjects</DashboardTable>
         <LinkID>cbc525b2-9826-47be-b4cb-6586f140e9a4</LinkID>
         <Title>This Quarter</Title>
         <LinkUrl />
         <DefaultLink>true</DefaultLink>
         <ScreenView>1</ScreenView>
         <IsHide>false</IsHide>
         <Order>0</Order>
         <UseAsPanel>false</UseAsPanel>
         <ExpressionFormat>$exp$</ExpressionFormat>
         <HideTitle>false</HideTitle>
         <DateFilterStartField>Created</DateFilterStartField>
         <DateFilterDefaultView>Current Quarter</DateFilterDefaultView>
         <Filter />
         <MaxLimit>100</MaxLimit>
         <DecimalPoint>0</DecimalPoint>
         <ShowBar>true</ShowBar>
         <AggragateFun>Count</AggragateFun>
         <AggragateOf>[ID]</AggragateOf>
         <BarDefaultColor>#59D782</BarDefaultColor>
         <IsPct>false</IsPct>
         <Conditions />
         <FontColor>#000000</FontColor>
         <BarUnit />
         <StopLinkDetail>false</StopLinkDetail>
      </DashboardPanelLink>
      <DashboardPanelLink>
         <PanelModuleType>All</PanelModuleType>
         <ExpressionID>0</ExpressionID>
         <FormulaId>0</FormulaId>
         <ViewType>0</ViewType>
         <ModuleName />
         <DashboardTable>TSKProjects</DashboardTable>
         <LinkID>16f060ab-01b9-44f8-9e3a-7a0ff8c7eb46</LinkID>
         <Title>Open Tasks</Title>
         <LinkUrl />
         <DefaultLink>true</DefaultLink>
         <ScreenView>1</ScreenView>
         <IsHide>false</IsHide>
         <Order>0</Order>
         <UseAsPanel>false</UseAsPanel>
         <ExpressionFormat>$exp$</ExpressionFormat>
         <HideTitle>false</HideTitle>
         <DateFilterStartField />
         <DateFilterDefaultView />
         <Filter>[TicketClosed] = ''0''</Filter>
         <MaxLimit>100</MaxLimit>
         <DecimalPoint>0</DecimalPoint>
         <ShowBar>true</ShowBar>
         <AggragateFun>Count</AggragateFun>
         <AggragateOf>[ID]</AggragateOf>
         <BarDefaultColor>#67C4C2</BarDefaultColor>
         <IsPct>false</IsPct>
         <Conditions />
         <FontColor>#000000</FontColor>
         <BarUnit />
         <StopLinkDetail>false</StopLinkDetail>
      </DashboardPanelLink>
   </Expressions>
   <PanelID>394a788c-f719-4ab7-a14c-45ca4039c12d</PanelID>
   <IconUrl>0</IconUrl>
   <ColumnViewType>0</ColumnViewType>
   <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 62, 200, 100, NULL, N'Accent1', N'Task Lists Date Wise', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.187' AS DateTime), CAST(N'2018-01-19 12:08:39.187' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (57, NULL, NULL, N'Last 3 Task Completed For Task', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Last 3 Task Completed For Task</ContainerTitle>
  <Description>Last 3 Task Completed For Task</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>6b11631b-513d-4d6e-a903-6152373265d0</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>0581dd72-a28c-43a3-a460-144144dea8ef</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Title Pick top 3 data points</Title>
      <SelectedField>Title</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>3</PickTopDataPoint>
      <DataPointOrder>Descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator />
      <OperatorField>TicketCloseDate</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>TSKProjects</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Title Pick top 3 data points</Title>
      <FactTable>TSKProjects</FactTable>
      <GroupByField />
      <Operator />
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Center</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>None</DataPointClickEvent>
      <FunctionExpression>TicketCloseDate</FunctionExpression>
      <Palette>Light</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>TSKProjects</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[TicketClosed] = ''1''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 63, 400, 200, NULL, N'Accent1', N'Last 3 Task Completed For Task', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.187' AS DateTime), CAST(N'2018-01-19 12:08:39.187' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (58, NULL, NULL, N'Task Completed Trend', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Task Completed Trend</ContainerTitle>
  <Description>Task Completed Trend</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>61de21f0-d34d-4ca7-aa3f-97d5b84a0325</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>d7225803-24a9-4fa8-b59f-3bac1fa112c3</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Close Date</Title>
      <SelectedField>TicketCloseDate</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>TSKProjects</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Task Completed Trend</Title>
      <FactTable>TSKProjects</FactTable>
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>TSKProjects</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 64, 300, 300, NULL, N'Accent1', N'Task Completed Trend', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.187' AS DateTime), CAST(N'2018-01-19 12:08:39.187' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (59, NULL, NULL, N'Completed Project Date Wise', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>Completed Project Date Wise</ContainerTitle>
  <Description>Completed Project Date Wise</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>8f3b073f-f8d8-4e91-8f70-d5b8ba2716a8</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>9af89026-56cd-41bd-a6c0-528faf959df1</LinkID>
      <Title>Last 30 Days</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField>TicketCloseDate</DateFilterStartField>
      <DateFilterDefaultView>Last 30 Days</DateFilterDefaultView>
      <Filter>[TicketClosed] = ''1''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#E6CC5D</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>b9cd828d-5be3-4712-b0c2-98972dab5bd6</LinkID>
      <Title>This Quarter</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField>TicketCloseDate</DateFilterStartField>
      <DateFilterDefaultView>Current Quarter</DateFilterDefaultView>
      <Filter> [TicketClosed] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#CCFFCC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>23c80803-43c4-440d-b899-c121828008b7</LinkID>
      <Title>Past 6 Months</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField>TicketCloseDate</DateFilterStartField>
      <DateFilterDefaultView>Past 6 Months</DateFilterDefaultView>
      <Filter>[TicketClosed] = ''1''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#CCFFFF</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>0a45f564-b16e-4595-862b-9559fab09e27</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 65, 200, 100, NULL, N'Accent1', N'Completed Project Date Wise', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.187' AS DateTime), CAST(N'2018-01-19 12:08:39.187' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (60, NULL, NULL, N'Closed Project Metrics', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>Closed Project Metrics</ContainerTitle>
  <Description>Closed Project Metrics</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>fafe64d8-462c-47d9-a0e0-6a7778d2a047</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>1ebfd0aa-6ad1-4ed5-af0f-232230714d54</LinkID>
      <Title>Avg. Days to Finish</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$ Days</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketClosed] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>avg</AggragateFun>
      <AggragateOf>[TicketDuration]</AggragateOf>
      <BarDefaultColor>#00CCFF</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Day</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>0b26237d-e296-4764-9fd1-ed51c40ac538</LinkID>
      <Title>Longest Project</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$ Days</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketClosed] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>avg</AggragateFun>
      <AggragateOf>[TicketDuration]</AggragateOf>
      <BarDefaultColor>#00CCFF</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Day</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>08673311-9d1e-4564-b08c-963b1973d479</LinkID>
      <Title>Average Size</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$ hrs</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketClosed] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>avg</AggragateFun>
      <AggragateOf>[TicketDuration] * 8</AggragateOf>
      <BarDefaultColor>#00FFFF</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Hour</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>ac65d935-9ca7-484d-8e74-c6043888f119</LinkID>
      <Title>Max Size</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$ hrs</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketClosed] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>max</AggragateFun>
      <AggragateOf>[TicketDuration] * 8</AggragateOf>
      <BarDefaultColor>#00FFFF</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Hour</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>c91764f0-b262-4444-912f-72be0550b626</LinkID>
      <Title>Avg. Cost</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>eval[$exp$/1000]k</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketClosed] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>avg</AggragateFun>
      <AggragateOf>[TicketTotalCost]</AggragateOf>
      <BarDefaultColor>#FFFF00</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Currency</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>5aa7c4c1-3f17-4d55-aeb3-2281042ab9c0</LinkID>
      <Title>Biggest Project</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>eval[$exp$/1000]k</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketClosed] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>avg</AggragateFun>
      <AggragateOf>[ProjectCost]</AggragateOf>
      <BarDefaultColor>#FFFF00</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Currency</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>a7cdc891-3750-4afc-ab1a-0b07f1a75aae</LinkID>
      <Title>% on Budget</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$%</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketTotalCost] &gt; [ProjectCost]</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#CCFFCC</BarDefaultColor>
      <IsPct>true</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Percentage</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>3a8355df-cdd5-43a4-8233-0a922f99c0ef</LinkID>
      <Title>Budget</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$eval[$exp$/1000]k</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter> [TicketClosed] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>sum</AggragateFun>
      <AggragateOf>[TicketTotalCost]</AggragateOf>
      <BarDefaultColor>#CCFFCC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Currency</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>8e284ddf-fa5b-4cae-9823-cfc5ea56d2b5</LinkID>
      <Title>Actuals</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$eval[$exp$/1000]k</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketClosed] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>sum</AggragateFun>
      <AggragateOf>[ProjectCost]</AggragateOf>
      <BarDefaultColor>#FFCC00</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Currency</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>e22e9061-aa73-4c16-9f64-ec4a76fe25d1</LinkID>
      <Title>Cost Variance</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$%</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter> [TicketClosed] = ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun />
      <AggragateOf>(Sum([TicketTotalCost])-Sum([ProjectCost]))/Sum([TicketTotalCost])</AggragateOf>
      <BarDefaultColor>#FFCC00</BarDefaultColor>
      <IsPct>true</IsPct>
      <Conditions>
        <KPIBarCondition>
          <Score>0</Score>
          <Operator>&lt;</Operator>
          <Color>#FF7F7F</Color>
        </KPIBarCondition>
      </Conditions>
      <FontColor>#000000</FontColor>
      <BarUnit>Percentage</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>bfdebb33-2562-4f18-a781-8061b457180d</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 66, 200, 250, NULL, N'Accent1', N'Closed Project Metrics', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.187' AS DateTime), CAST(N'2018-01-19 12:08:39.187' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (61, NULL, NULL, N'On-Going Projects', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>On-Going Projects</ContainerTitle>
  <Description>On-Going Projects</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>23c4cbda-bee2-4195-b36f-048285bc2caf</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>f5b6aae5-d439-46e8-87b8-560ea0219fc4</LinkID>
      <Title>% Work Done</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$%</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketClosed] &lt;&gt; ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>avg</AggragateFun>
      <AggragateOf>[TicketPctComplete] * 100</AggragateOf>
      <BarDefaultColor>#CCFFFF</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Percentage</BarUnit>
      <StopLinkDetail>true</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>27eb0c34-8d88-481e-8780-6405b82b4a0a</LinkID>
      <Title>Days Remaining</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$ Days</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketClosed] &lt;&gt; ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>sum</AggragateFun>
      <AggragateOf>f:daysdiff([TicketActualCompletionDate],[Today],1)</AggragateOf>
      <BarDefaultColor>#CCFFCC</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Day</BarUnit>
      <StopLinkDetail>true</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMMProjects</DashboardTable>
      <LinkID>ac0edd2e-a1dd-46c6-b7ac-91710fb47733</LinkID>
      <Title>Days to Complete</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$ Days</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketClosed] &lt;&gt; ''1'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>sum</AggragateFun>
      <AggragateOf>[UGITDaysToComplete]</AggragateOf>
      <BarDefaultColor>#FFCC99</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Day</BarUnit>
      <StopLinkDetail>true</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>678f7596-40a5-433e-825f-ecff3c75d2ad</PanelID>
  <IconUrl />
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 67, 200, 100, NULL, N'Accent1', N'On-Going Projects', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.187' AS DateTime), CAST(N'2018-01-19 12:08:39.187' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (62, NULL, NULL, N'Top 5 Assets', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Top 5 Assets</ContainerTitle>
  <Description>Top 5 Assets</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>22c47b70-f68a-4caa-ab3a-5eb781a7c88f</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>fdb66072-15db-403d-affb-05ba6788f36b</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Top 5</Title>
      <SelectedField>BudgetSubCategory</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator />
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>Assets</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Top 5</Title>
      <FactTable>Assets</FactTable>
      <GroupByField />
      <Operator />
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Center</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>BrightPastel</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>Assets</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 68, 200, 120, NULL, N'Accent1', N'Top 5 Assets', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.187' AS DateTime), CAST(N'2018-01-19 12:08:39.187' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (63, NULL, NULL, N'Assets with Most Issues', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Assets with Most Issues</ContainerTitle>
  <Description>Assets with Most Issues</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>d2fa1f1a-786a-40c5-a72e-81da0131d7c3</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>8e6c5a75-86df-4b77-b8c5-104b287c29ab</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Top 5</Title>
      <SelectedField>BudgetSubCategory</SelectedField>
      <Sequence>0</Sequence>
      <PickTopDataPoint>5</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>sum</Operator>
      <OperatorField>IssueCount</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>Assets</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Top 5</Title>
      <FactTable>Assets</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Bar</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Center</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>Assets</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[IssueCount] &gt; 0</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 69, 200, 120, NULL, N'Accent1', N'Assets with Most Issues', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.187' AS DateTime), CAST(N'2018-01-19 12:08:39.187' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (64, NULL, N'', N'Age of PCs', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>Age of PCs</ContainerTitle>
  <Description>Age of PCs</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>9d15757c-897c-46f0-b534-e1dc4e953759</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>Assets</DashboardTable>
      <LinkID>402fd4c9-b571-441b-ac5a-bb3ce95341bf</LinkID>
      <Title>&lt; 1 Yr</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$%</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>f:yearsdiff([Today],[StartDate]) &lt; 1</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Percentage</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>Assets</DashboardTable>
      <LinkID>ebaeb6db-04cc-423e-b49d-884e954b5edf</LinkID>
      <Title>1-2 Yr</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$%</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>f:yearsdiff([Today],[StartDate]) &gt;=1 and f:yearsdiff([Today],[StartDate]) &lt;= 2</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Percentage</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>Assets</DashboardTable>
      <LinkID>f1d62ed2-36cc-4fb0-b001-7dcaac096fac</LinkID>
      <Title>3-5 Yr</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$%</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>f:yearsdiff([Today],[StartDate])  &gt;= 3 and f:yearsdiff([Today],[StartDate]) &lt;= 5</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Percentage</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>Assets</DashboardTable>
      <LinkID>b5b58075-3d7d-44e2-8fb3-8d8f7e20814b</LinkID>
      <Title>&gt;5 Yr</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$%</ExpressionFormat>
      <HideTitle>false</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>f:yearsdiff([Today],[StartDate]) &gt; 5</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>true</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>Percentage</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>a8544d29-f21d-458c-bb7f-42c468ec99f6</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', N'', 0, N'', N'', 0, 0, 0, 0, 68, 200, 120, N'', N'Accent1', N'Age of PCs', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.190' AS DateTime), CAST(N'2018-01-19 12:08:39.190' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (65, NULL, NULL, N'MyWikis', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>MyWikis</ContainerTitle>
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>4a2bdb1a-4d0e-4fb2-a70c-627f8e5310bf</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>WikiArticles</DashboardTable>
      <LinkID>112c4dbf-4218-4346-8921-98491eeb60d7</LinkID>
      <Title>My Wikis</Title>
      <LinkUrl>/SitePages/WikiArticles.aspx</LinkUrl>
      <DefaultLink>false</DefaultLink>
      <ScreenView>0</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>true</UseAsPanel>
      <ExpressionFormat>My Wikis ($exp$)</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[Author] = ''[me]'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[TicketId]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>733f8608-a9d3-4a57-8e40-7d0f4f26fc4e</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 71, 300, 300, NULL, N'Accent1', N'MyWikis', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.190' AS DateTime), CAST(N'2018-01-19 12:08:39.190' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (66, NULL, NULL, N'Popular Wikis', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>Popular Wikis</ContainerTitle>
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>1b3dd53a-b207-4cf7-9948-a35d0327bac2</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>WikiArticles</DashboardTable>
      <LinkID>b848206f-b37e-4ad9-8f08-b482a9197b9e</LinkID>
      <Title>Popular Wikis</Title>
      <LinkUrl>/SitePages/WikiArticles.aspx</LinkUrl>
      <DefaultLink>false</DefaultLink>
      <ScreenView>0</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>true</UseAsPanel>
      <ExpressionFormat>Popular Wikis ($exp$)</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[WikiViews] &gt; 10</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[TicketId]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>c254e20f-864c-48f1-8738-34c74c0e38a0</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 72, 300, 300, NULL, N'Accent1', N'Popular Wikis', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.190' AS DateTime), CAST(N'2018-01-19 12:08:39.190' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (67, NULL, NULL, NULL, NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>Favorite Wikis</ContainerTitle>
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>4897474f-13e8-40bb-8183-a7682d319171</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>WikiReview</DashboardTable>
      <LinkID>d6c08377-4387-4064-a541-be5115cc0381</LinkID>
      <Title>Favorite Wikis</Title>
      <LinkUrl>/SitePages/WikiArticles.aspx</LinkUrl>
      <DefaultLink>false</DefaultLink>
      <ScreenView>0</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>true</UseAsPanel>
      <ExpressionFormat>Favorite Wikis ($exp$)</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[WikiUserType] = ''favorite'' AND [Author] = ''[me]''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[TicketId]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>896c2ae1-34e2-4602-9f87-fe3d0689ddbf</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, 1, NULL, NULL, 73, 300, 300, NULL, N'Accent1', N'Favorite Wikis', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.190' AS DateTime), CAST(N'2018-01-19 12:08:39.190' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (68, NULL, NULL, N'All Wikis', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>All Wikis</ContainerTitle>
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>4388a173-b96c-4411-adbc-8206e16575ec</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>WikiArticles</DashboardTable>
      <LinkID>354a8c5d-93fd-4dc5-b9cc-7daef278ba38</LinkID>
      <Title>All Wikis</Title>
      <LinkUrl>/SitePages/WikiArticles.aspx</LinkUrl>
      <DefaultLink>false</DefaultLink>
      <ScreenView>0</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>true</UseAsPanel>
      <ExpressionFormat>All Wikis ($exp$)</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[TicketId]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>1b87b139-a58d-4996-8734-a2df2b517ab6</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 74, 100, 100, NULL, N'Accent1', N'All Wikis', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.190' AS DateTime), CAST(N'2018-01-19 12:08:39.190' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (69, NULL, NULL, N'My Projects', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>My Projects</ContainerTitle>
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>c10ea90a-4f90-4160-97bd-3213f3ca71a2</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>PMM-OpenTickets</DashboardTable>
      <LinkID>6b0aeb64-bce7-4edd-9fd2-0327ad907a81</LinkID>
      <Title>My Projects</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>true</UseAsPanel>
      <ExpressionFormat>My Projects ($exp$)</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[TicketSponsors] Like ''%[Me]%'' OR [TicketStakeHolders] Like ''%[Me]%'' OR  [TicketProjectManager] Like ''%[Me]%''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>66f6dab1-72de-494b-a779-c5792496e347</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 75, 100, 100, NULL, N'Accent1', N'My Projects', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.190' AS DateTime), CAST(N'2018-01-19 12:08:39.190' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (70, NULL, NULL, N'Wikis', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>Wikis</ContainerTitle>
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>3f213342-fcf3-43a8-b052-d4d037f83478</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>WikiArticles</DashboardTable>
      <LinkID>651a705c-df16-4fc2-a202-15b34984bae4</LinkID>
      <Title>Wikis</Title>
      <LinkUrl>/SitePages/WikiArticles.aspx</LinkUrl>
      <DefaultLink>false</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>true</UseAsPanel>
      <ExpressionFormat>Wikis ($exp$)</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>6cede5e0-956f-43e3-b0a2-86060b04f164</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 76, 100, 100, NULL, N'Accent1', N'Wikis', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.190' AS DateTime), CAST(N'2018-01-19 12:08:39.190' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (71, NULL, N'', N'IT Assets', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>IT Assets</ContainerTitle>
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>c0b310cd-2be9-473b-8b43-cb8953a72efe</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>Assets</DashboardTable>
      <LinkID>aa60b6ce-b67d-4987-83d0-d950e91ddc5a</LinkID>
      <Title>Assets</Title>
      <LinkUrl>/SitePages/CMDB.aspx</LinkUrl>
      <DefaultLink>false</DefaultLink>
      <ScreenView>0</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>true</UseAsPanel>
      <ExpressionFormat>IT Assets ($exp$)</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter>[IsDeleted] = ''False''</Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>c4a47bb9-aa3a-4319-b0f2-2c48efbc8ac2</PanelID>
  <IconUrl>0</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', N'', 0, N'Bold;#8pt;##000000', N'Bold;#8pt;##000000', 0, 0, 0, 0, 77, 200, 200, N'', N'Accent1', N'IT Assets', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.190' AS DateTime), CAST(N'2018-01-19 12:08:39.190' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (72, NULL, NULL, N'PRS Tickets by Functional Area', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>PRS Tickets by Functional Area</ContainerTitle>
  <Description>PRS Tickets by Functional Area</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>881b2c34-819b-4dab-9525-80a0b8b53c88</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>0f9dcb76-0624-4c24-955a-aa52cb300df7</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Functional Area</Title>
      <SelectedField>FunctionalAreaLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priority</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Past 6 Months</BasicDateFilterDefaultView>
  <BasicFilter>ModuleNameLookup = ''PRS''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 78, 600, 500, NULL, N'Accent1', N'PRS Tickets by Functional Area', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.190' AS DateTime), CAST(N'2018-01-19 12:08:39.190' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (73, NULL, NULL, N'PRS Tickets by Location', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>PRS Tickets by Location</ContainerTitle>
  <Description>PRS Tickets by Location</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>074bdf70-c2b0-4f7f-bce0-56736aa494b2</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>0f9dcb76-0624-4c24-955a-aa52cb300df7</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Location</Title>
      <SelectedField>LocationLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priority</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Past 6 Months</BasicDateFilterDefaultView>
  <BasicFilter>ModuleNameLookup = ''PRS''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 79, 600, 500, NULL, N'Accent1', N'PRS Tickets by Location', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.190' AS DateTime), CAST(N'2018-01-19 12:08:39.190' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (74, NULL, NULL, N'Tickets by Location', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>dd559442-b5e9-41a6-9e9c-afde36daa006</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#E7EAFE</BGColor>
  <Id />
  <QueryId>dd559442-b5e9-41a6-9e9c-afde36daa006</QueryId>
  <QueryTable />
  <QueryInfo>
    <GroupBy>
      <GroupByInfo>
        <Column>
          <ID>8</ID>
          <FieldName>LocationLookup</FieldName>
          <DisplayName>Location</DisplayName>
          <DataType>String</DataType>
          <TableName>DashboardSummary</TableName>
          <Function>none</Function>
          <Sequence>11</Sequence>
          <Selected>true</Selected>
          <Hidden>false</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
        </Column>
        <Num>1</Num>
      </GroupByInfo>
    </GroupBy>
    <OrderBy>
      <OrderByInfo>
        <Num>1</Num>
        <Column>
          <ID>23</ID>
          <FieldName>TicketId</FieldName>
          <DisplayName>Ticket ID</DisplayName>
          <DataType>String</DataType>
          <TableName>DashboardSummary</TableName>
          <Function>none</Function>
          <Sequence>1</Sequence>
          <Selected>true</Selected>
          <Hidden>false</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
        </Column>
        <orderBy>ASC</orderBy>
      </OrderByInfo>
    </OrderBy>
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns>
          <ColumnInfo>
            <ID>9</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>48</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>2</ID>
            <FieldName>AssignedDate</FieldName>
            <DisplayName>Assigned Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>7</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>4</ID>
            <FieldName>Category</FieldName>
            <DisplayName>Category</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>5</ID>
            <FieldName>ClosedDate</FieldName>
            <DisplayName>Closed Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>9</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>7</ID>
            <FieldName>FunctionalAreaLookup</FieldName>
            <DisplayName>Functional Area Lookup</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>10</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>8</ID>
            <FieldName>GenericStatusLookup</FieldName>
            <DisplayName>Generic Status</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>13</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>11</ID>
            <FieldName>LocationLookup</FieldName>
            <DisplayName>Location</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>11</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>20</ID>
            <FieldName>ResolvedDate</FieldName>
            <DisplayName>Resolved Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>8</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>28</ID>
            <FieldName>TicketCreationDate</FieldName>
            <DisplayName>Created</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>6</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>29</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>Ticket ID</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>31</ID>
            <FieldName>TicketInitiatorResolved</FieldName>
            <DisplayName>Initiator Resolved</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>15</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>32</ID>
            <FieldName>TicketOnHold</FieldName>
            <DisplayName>On Hold</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>16</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>34</ID>
            <FieldName>TicketOwner</FieldName>
            <DisplayName>Owner</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>17</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>35</ID>
            <FieldName>TicketPriorityLookup</FieldName>
            <DisplayName>Priority</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>12</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>36</ID>
            <FieldName>TicketPRP</FieldName>
            <DisplayName>PRP</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>18</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>39</ID>
            <FieldName>TicketRequestSource</FieldName>
            <DisplayName>Request Source</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>5</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>40</ID>
            <FieldName>TicketRequestTypeLookup</FieldName>
            <DisplayName>Request Type</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>43</ID>
            <FieldName>TicketStatus</FieldName>
            <DisplayName>Status</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>14</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>44</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>DashboardSummary.ModuleNameLookup</ColumnName>
        <DataType>String</DataType>
        <Operator>Equal</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>TSR</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals>
      <ColumnInfo>
        <ID>10</ID>
        <FieldName>TicketId</FieldName>
        <DisplayName>Ticket ID</DisplayName>
        <DataType>String</DataType>
        <TableName>DashboardSummary</TableName>
        <Function>Count</Function>
        <Sequence>1</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
    </Totals>
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 80, 0, 0, NULL, N'Accent1', N'Tickets by Location', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.190' AS DateTime), CAST(N'2018-01-19 12:08:39.190' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (75, NULL, NULL, N'Actual vs Allocated by Category', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>$Date$ Actual vs Allocated by Category (Hours)</ContainerTitle>
  <Description>Actual vs Allocated by Category</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>f4e10ea6-ceb6-4490-b83a-f20e3bc5a01c</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>9a36e9e2-10ad-417f-9abd-c89f56186c27</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Category</Title>
      <SelectedField>WorkItemType</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Sub-Category</Title>
      <SelectedField>WorkItem</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Allocation Hours</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>true</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>[AllocationHour]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Actual Hours</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>true</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ActualHour</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ResourceUsageSummaryMonthWise</FactTable>
  <BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Current Month</BasicDateFilterDefaultView>
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 81, 500, 400, NULL, N'Accent1', N'$Date$ Actual vs Allocated by Category (Hours)', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.430' AS DateTime), CAST(N'2018-01-19 12:08:39.430' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (76, NULL, NULL, N'Actual vs Allocated by Manager (FTE)', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>$Date$ Actual vs Allocated by Manager (FTE)</ContainerTitle>
  <Description>Actual vs Allocated by Manager</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>a6a2518c-ec64-4c70-b7a8-05d76ded74cb</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>1457862b-f6b7-40d1-b2be-a24f06142c82</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Resource</Title>
      <SelectedField>Resource</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Category</Title>
      <SelectedField>WorkItemType</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Allocation FTEs</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>PctAllocation/100</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Actual FTEs</Title>
      <FactTable>ResourceUsageSummaryMonthWise</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>PctActual/100</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ResourceUsageSummaryMonthWise</FactTable>
  <BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Current Month</BasicDateFilterDefaultView>
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 82, 500, 400, NULL, N'Accent1', N'$Date$ Actual vs Allocated by Manager (FTE)', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.430' AS DateTime), CAST(N'2018-01-19 12:08:39.430' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (77, NULL, NULL, N'Open PRS Tickets By Age', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Open PRS Tickets By Age</ContainerTitle>
  <Description>Open PRS Tickets By Age &amp; Functional Area</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>76a80f80-8dab-48c9-b361-cee9e72d0da0</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>8ffea3de-ec51-4728-bb3d-1ea253cc6586</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Functional Area</Title>
      <SelectedField>FunctionalAreaLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>10</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>TOTAL PRS Tickets</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P1 Tickets 0-3 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''1-High'' and f:Daysdiff([TicketCreationDate],[Today]) &lt; 3</ExpressionFormula>
      <Order>2</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P1 Tickets 3-5 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''1-High'' and f:Daysdiff([TicketCreationDate],[Today]) &gt;= 3 and f:Daysdiff([TicketCreationDate],[Today]) &lt; 5</ExpressionFormula>
      <Order>3</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P1 Tickets &gt; 5 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''1-High'' and f:Daysdiff([TicketCreationDate],[Today]) &gt;=5</ExpressionFormula>
      <Order>4</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P2 Tickets 0-3 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''2-Medium'' and f:Daysdiff([TicketCreationDate],[Today]) &lt; 3</ExpressionFormula>
      <Order>5</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P2 Ticket 3-5 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''2-Medium'' and f:Daysdiff([TicketCreationDate],[Today]) &gt;=3 and f:Daysdiff([TicketCreationDate],[Today]) &lt; 5</ExpressionFormula>
      <Order>6</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P2 Tickets &gt; 5 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''2-Medium'' and f:Daysdiff([TicketCreationDate],[Today]) &gt;= 5</ExpressionFormula>
      <Order>7</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P3 Tickets 0-3 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''3-Low'' and f:Daysdiff([TicketCreationDate],[Today]) &lt;3</ExpressionFormula>
      <Order>8</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P3 Tickets 3-5 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''3-Low'' and f:Daysdiff([TicketCreationDate],[Today]) &gt;=3 and f:Daysdiff([TicketCreationDate],[Today]) &lt;5</ExpressionFormula>
      <Order>9</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P3 Tickets &gt; 5 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''3-Low'' and f:Daysdiff([TicketCreationDate],[Today]) &gt;=5</ExpressionFormula>
      <Order>10</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[ModuleNameLookup] = ''PRS'' AND [GenericStatusLookup] &lt;&gt; ''Closed''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>true</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 83, 500, 400, NULL, N'Accent1', N'Open PRS Tickets By Age', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.430' AS DateTime), CAST(N'2018-01-19 12:08:39.430' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (78, NULL, NULL, N'Open TSR Tickets By Age', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Open TSR Tickets By Age</ContainerTitle>
  <Description>Open TSR Tickets By Age &amp; Functional Area</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>eaf6c1bc-54ac-47cd-9742-bd003a99b95f</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#E7EAFE</BGColor>
  <Id />
  <ChartId>8ffea3de-ec51-4728-bb3d-1ea253cc6586</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Functional Area</Title>
      <SelectedField>FunctionalAreaLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>10</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>TOTAL TSR Tickets</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions>
        <string>Functional Area</string>
      </Dimensions>
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P1 Tickets 0-3 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''1-High'' and f:Daysdiff([TicketCreationDate],[Today]) &lt; 3</ExpressionFormula>
      <Order>2</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions>
        <string>Functional Area</string>
      </Dimensions>
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P1 Tickets 3-5 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''1-High'' and f:Daysdiff([TicketCreationDate],[Today]) &gt;= 3 and f:Daysdiff([TicketCreationDate],[Today]) &lt; 5</ExpressionFormula>
      <Order>3</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions>
        <string>Functional Area</string>
      </Dimensions>
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P1 Tickets &gt; 5 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''1-High'' and f:Daysdiff([TicketCreationDate],[Today]) &gt;=5</ExpressionFormula>
      <Order>4</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions>
        <string>Functional Area</string>
      </Dimensions>
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P2 Tickets 0-3 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''2-Medium'' and f:Daysdiff([TicketCreationDate],[Today]) &lt; 3</ExpressionFormula>
      <Order>5</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions>
        <string>Functional Area</string>
      </Dimensions>
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P2 Ticket 3-5 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''2-Medium'' and f:Daysdiff([TicketCreationDate],[Today]) &gt;=3 and f:Daysdiff([TicketCreationDate],[Today]) &lt; 5</ExpressionFormula>
      <Order>6</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions>
        <string>Functional Area</string>
      </Dimensions>
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P2 Tickets &gt; 5 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''2-Medium'' and f:Daysdiff([TicketCreationDate],[Today]) &gt;= 5</ExpressionFormula>
      <Order>7</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions>
        <string>Functional Area</string>
      </Dimensions>
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P3 Tickets 0-3 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''3-Low'' and f:Daysdiff([TicketCreationDate],[Today]) &lt;3</ExpressionFormula>
      <Order>8</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions>
        <string>Functional Area</string>
      </Dimensions>
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P3 Tickets 3-5 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''3-Low'' and f:Daysdiff([TicketCreationDate],[Today]) &gt;=3 and f:Daysdiff([TicketCreationDate],[Today]) &lt;5</ExpressionFormula>
      <Order>9</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions>
        <string>Functional Area</string>
      </Dimensions>
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>P3 Tickets &gt; 5 Days</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula>[TicketPriorityLookup] = ''3-Low'' and f:Daysdiff([TicketCreationDate],[Today]) &gt;=5</ExpressionFormula>
      <Order>10</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions>
        <string>Functional Area</string>
      </Dimensions>
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[ModuleNameLookup] = ''TSR'' AND [GenericStatusLookup] &lt;&gt; ''Closed''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>true</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 84, 500, 400, NULL, N'Accent1', N'Open TSR Tickets By Age', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.433' AS DateTime), CAST(N'2018-01-19 12:08:39.433' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (79, NULL, N'Ticketing', N'Tickets closed by month', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Tickets closed by month</ContainerTitle>
  <Description>Tickets closed by month</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>a8840247-aa16-4717-b518-775bf43a1fd6</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>c44d8e82-d7e0-4a13-9c0e-b3437dace652</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Closed Tickets</Title>
      <SelectedField>ClosedDate</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>6</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator />
      <OperatorField>ClosedDate</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Closed tickets</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>ClosedDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Past 6 Months</BasicDateFilterDefaultView>
  <BasicFilter>[TicketClosed] = 1</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 85, 300, 300, N'SMS', N'Accent1', N'Tickets closed by month', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.433' AS DateTime), CAST(N'2018-01-19 12:08:39.433' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (80, NULL, N'', N'TSR Tickets By Location', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>TSR Tickets By Location</ContainerTitle>
  <Description>TSR Tickets By Location</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>6b2f70f8-9bd4-4ffe-9388-283fee2b599e</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>8ac3f4a0-5e34-42a9-9b71-790055048adc</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Location</Title>
      <SelectedField>LocationLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priority</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>CreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField>CreationDate</BasicDateFitlerEndField>
  <BasicDateFilterDefaultView />
  <BasicFilter>[ModuleNameLookup] = ''TSR''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <BGColor>#E7EAFE</BGColor>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 85, 500, 500, N'', N'Accent1', N'TSR Tickets By Location', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.433' AS DateTime), CAST(N'2018-01-19 12:08:39.433' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (81, NULL, NULL, N'PRS Tickets by Company', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>PRS Tickets by Company</ContainerTitle>
  <Description>PRS Tickets by Company</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>f7208ba9-d3d4-41fe-91ab-02d15073aa7f</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>0f9dcb76-0624-4c24-955a-aa52cb300df7</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Company</Title>
      <SelectedField>RequestorCompany</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Division</Title>
      <SelectedField>RequestorDivision</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Department</Title>
      <SelectedField>RequestorDepartment</SelectedField>
      <Sequence>3</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priority</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Past 6 Months</BasicDateFilterDefaultView>
  <BasicFilter>ModuleNameLookup = ''PRS''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 87, 600, 500, NULL, N'Accent1', N'PRS Tickets by Company', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.433' AS DateTime), CAST(N'2018-01-19 12:08:39.433' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (82, NULL, NULL, N'TSR Tickets by Company', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>TSR Tickets by Company</ContainerTitle>
  <Description>TSR Tickets by Company</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>60a53c78-4734-4d06-831c-7c295d54284e</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>0f9dcb76-0624-4c24-955a-aa52cb300df7</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Company</Title>
      <SelectedField>RequestorCompany</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Division</Title>
      <SelectedField>RequestorDivision</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Department</Title>
      <SelectedField>RequestorDepartment</SelectedField>
      <Sequence>3</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priority</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView>Past 6 Months</BasicDateFilterDefaultView>
  <BasicFilter>ModuleNameLookup = ''TSR''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 88, 600, 500, NULL, N'Accent1', N'TSR Tickets by Company', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.437' AS DateTime), CAST(N'2018-01-19 12:08:39.437' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (83, NULL, NULL, N'TSR Tickets by Functional Area', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>TSR Tickets by Functional Area</ContainerTitle>
  <Description>TSR Tickets by Functional Area</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>d1053bc0-8c0e-405f-84cd-1416ac6518a8</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#E7EAFE</BGColor>
  <Id />
  <ChartId>0f9dcb76-0624-4c24-955a-aa52cb300df7</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Functional Area</Title>
      <SelectedField>FunctionalAreaLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Priority</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField>TicketPriorityLookup</GroupByField>
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>StackedColumn</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>ModuleNameLookup = ''TSR''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>-30</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 89, 600, 500, NULL, N'Accent1', N'TSR Tickets by Functional Area', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.440' AS DateTime), CAST(N'2018-01-19 12:08:39.440' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (84, NULL, NULL, N'Sprint Burndown Chart', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Sprint Burndown Chart</ContainerTitle>
  <Description>Sprint Burndown Chart</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>ecbfdcd5-1168-4025-ba41-b971db2d1dfb</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>b5d0bbde-2a8b-4c63-8d07-3bcd3684ab83</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Category</Title>
      <SelectedField>ServiceCategoryName</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>10</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Service</Title>
      <SelectedField>ServiceName</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>10</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>DashboardSummary</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Ticket Count</Title>
      <FactTable>DashboardSummary</FactTable>
      <GroupByField />
      <Operator>Count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette>Bright</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[ServiceName] &lt;&gt; ''''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 90, 500, 500, NULL, N'Accent1', N'Sprint Burndown Chart', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.440' AS DateTime), CAST(N'2018-01-19 12:08:39.440' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (85, NULL, NULL, N'VND Statistics', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>VND Statistics</ContainerTitle>
  <Description>VND Statistics</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>2f827eec-c3fe-4fb2-8604-53ef257d1b31</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>VND-OpenTickets</DashboardTable>
      <LinkID>38ee6f0f-3189-4e39-8932-6c866555f76c</LinkID>
      <Title>Active MSAs</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Active MSAs: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>VSW-OpenTickets</DashboardTable>
      <LinkID>94fb4446-5082-4f34-b4c6-ec05bbfb8dc8</LinkID>
      <Title>Active SOWs</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>2</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Active SOWs: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>9e9366b2-2de5-48fc-bb0c-5ae8802c9be0</PanelID>
  <IconUrl>/_layouts/images/uGovernIT/VND_32x32.png</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, 1, NULL, NULL, 91, 200, 175, NULL, N'Accent1', N'VND Statistics', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.447' AS DateTime), CAST(N'2018-01-19 12:08:39.447' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (86, NULL, NULL, N'VND Statistics 2', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <type>Panel</type>
  <ContainerTitle>VND Statistics 2</ContainerTitle>
  <Description>VND Statistics 2</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>4be75063-f7e5-419d-ada9-a9f235c12dec</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>VPM-OpenTickets</DashboardTable>
      <LinkID>3424bb02-b32b-47ee-9a6a-30b98c8080f8</LinkID>
      <Title>Perf Reports</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Perf Reports: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>VFM-OpenTickets</DashboardTable>
      <LinkID>807f6c2d-cd9b-4f12-88b7-bf42b370c747</LinkID>
      <Title>Invoices</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>2</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Invoices: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
    </DashboardPanelLink>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>VendorIssues</DashboardTable>
      <LinkID>691d1079-6ea0-42b0-bb7d-5ee8369303cf</LinkID>
      <Title>Issues</Title>
      <LinkUrl />
      <DefaultLink>false</DefaultLink>
      <ScreenView>0</ScreenView>
      <IsHide>false</IsHide>
      <Order>3</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Issues: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter> [Status] &lt;&gt; ''Completed'' </Filter>
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>true</StopLinkDetail>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>9e9366b2-2de5-48fc-bb0c-5ae8802c9be0</PanelID>
  <IconUrl>/_layouts/images/uGovernIT/ButtonImages/ITReport.png</IconUrl>
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 92, 200, 175, NULL, N'Accent1', N'VND Statistics 2', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.447' AS DateTime), CAST(N'2018-01-19 12:08:39.447' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (87, NULL, NULL, N'Vendor SLA Performance', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Vendor SLA Performance</ContainerTitle>
  <Description>Vendor SLA Performance</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>b12c24da-0dd9-49c5-8477-ae5d4b2dc329</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>11962ba7-0aa8-42f1-8eb3-eb7ad02df0da</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Report Month</Title>
      <SelectedField>VendorSLAReportingDate</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>Author</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>VendorSLAPerformance</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType>month</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Actual</Title>
      <FactTable>VendorSLAPerformance</FactTable>
      <GroupByField />
      <Operator>avg</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>VendorSLAPerformanceNumber</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Minimum</Title>
      <FactTable>VendorSLAPerformance</FactTable>
      <GroupByField />
      <Operator>avg</Operator>
      <ExpressionFormula />
      <Order>2</Order>
      <ChartType>Line</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>MinThreshold</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Target</Title>
      <FactTable>VendorSLAPerformance</FactTable>
      <GroupByField />
      <Operator>avg</Operator>
      <ExpressionFormula />
      <Order>3</Order>
      <ChartType>Line</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>SLATarget</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 93, 300, 300, NULL, N'Accent1', N'Vendor SLA Performance', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.450' AS DateTime), CAST(N'2018-01-19 12:08:39.450' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (88, NULL, NULL, N'Vendor Budget vs Actual Trend', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Vendor Budget vs Actual Trend</ContainerTitle>
  <Description>Vendor Budget vs Actual Trend</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>d545caad-bf06-42cf-b0cc-19866e5b1430</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>117f255b-ebab-40ad-b041-843a7302184c</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Year</Title>
      <SelectedField>SOWInvoiceDate</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>VendorSOWInvoices</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Vendor</Title>
      <SelectedField>VendorMSANameLookup</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>VendorSOWInvoices</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Month</Title>
      <SelectedField>SOWInvoiceDate</SelectedField>
      <Sequence>3</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>VendorSOWInvoices</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Invoice</Title>
      <SelectedField>SOWInvoiceDate</SelectedField>
      <Sequence>4</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>VendorSOWInvoices</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Budget</Title>
      <FactTable>VendorSOWInvoices</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>BudgetAmount</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Actual</Title>
      <FactTable>VendorSOWInvoices</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>SOWInvoiceActualAmount</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>lineMarkType</Key>
          <Value xsi:type="xsd:string">Circle</Value>
        </DataItem>
        <DataItem>
          <Key>lineMarkSize</Key>
          <Value xsi:type="xsd:string">10</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title>Variance</Title>
      <FactTable>VendorSOWInvoices</FactTable>
      <GroupByField />
      <Operator>variance</Operator>
      <ExpressionFormula />
      <Order>3</Order>
      <ChartType>Line</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Secondary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>[BudgetAmount],[SOWInvoiceActualAmount],[BudgetAmount]</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 94, 300, 300, NULL, N'Accent1', N'Vendor Budget vs Actual Trend', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.450' AS DateTime), CAST(N'2018-01-19 12:08:39.450' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (89, NULL, NULL, N'Vendor Invoice Details', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Vendor Invoice Details</ContainerTitle>
  <Description>Vendor Invoice Details</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>d34eb441-3567-4c98-b031-e4f556448bb7</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>5174f0ce-5730-4b65-8ae3-a4db03d19345</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Vendor</Title>
      <SelectedField>VendorMSANameLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>VendorSOWInvoiceDetail</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>SOW</Title>
      <SelectedField>VendorSOWNameLookup</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>VendorSOWInvoiceDetail</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Invoice</Title>
      <SelectedField>SOWInvoiceDate</SelectedField>
      <Sequence>3</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>VendorSOWInvoiceDetail</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>None</DataPointClickEvent>
      <DateViewType>day</DateViewType>
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Fixed</Title>
      <FactTable>VendorSOWInvoiceDetail</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>FixedFees</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Variable</Title>
      <FactTable>VendorSOWInvoiceDetail</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>2</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>VariableAmount</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 95, 300, 300, NULL, N'Accent1', N'Vendor Invoice Details', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.450' AS DateTime), CAST(N'2018-01-19 12:08:39.450' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (90, NULL, NULL, N'Vendor Budget vs Actual', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Vendor Budget vs Actual</ContainerTitle>
  <Description>Vendor Budget vs Actual</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>ef1c35d5-0efb-401a-bdaa-5130179843e9</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>117f255b-ebab-40ad-b041-843a7302184c</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Vendor</Title>
      <SelectedField>VendorMSANameLookup</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>VendorSOWInvoiceDetail</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Month</Title>
      <SelectedField>SOWInvoiceDate</SelectedField>
      <Sequence>3</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>VendorSOWInvoiceDetail</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
    <ChartDimension>
      <Title>Invoice</Title>
      <SelectedField>SOWInvoiceDate</SelectedField>
      <Sequence>4</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>Ascending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>Count</Operator>
      <OperatorField>ID</OperatorField>
      <IsCumulative>false</IsCumulative>
      <FactTable>VendorSOWInvoiceDetail</FactTable>
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Budget</Title>
      <FactTable>VendorSOWInvoiceDetail</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>BudgetAmount</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
    <ChartExpression>
      <ChartLevelProperties />
      <Title>Actual</Title>
      <FactTable>VendorSOWInvoiceDetail</FactTable>
      <GroupByField />
      <Operator>sum</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle>Auto</LabelStyle>
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>InvoiceItemAmount</FunctionExpression>
      <Palette>None</Palette>
      <DrawingStyle>Cylinder</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>true</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <LegendAlignment>Near</LegendAlignment>
  <LegendDocking>Top</LegendDocking>
  <BorderWidth>1</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 96, 300, 300, NULL, N'Accent1', N'Vendor Budget vs Actual', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.453' AS DateTime), CAST(N'2018-01-19 12:08:39.453' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (91, NULL, N'Project Management', N'Number Of Projects', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>2f0d03a6-7ca1-42b8-997d-96223e1f2fd8</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>2f0d03a6-7ca1-42b8-997d-96223e1f2fd8</QueryId>
  <QueryTable>PMMProjects</QueryTable>
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>PMMProjects</Name>
        <Columns>
          <ColumnInfo>
            <ID>44</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>PMMProjects</TableName>
            <Sequence>147</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>65</ID>
            <FieldName>ProjectRank</FieldName>
            <DisplayName>Project Rank</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>Count</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses />
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>PMMProjects</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 97, 0, 0, NULL, N'Accent1', N'Number Of Projects', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.457' AS DateTime), CAST(N'2018-01-19 12:08:39.457' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (92, NULL, N'none', N'New Tickets', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>04991b32-7a99-4887-8d92-87bdab293701</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>04991b32-7a99-4887-8d92-87bdab293701</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns>
          <ColumnInfo>
            <ID>9</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>Count</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>29</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>TicketId</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>48</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>DashboardSummary.GenericStatusLookup</ColumnName>
        <DataType>String</DataType>
        <Operator>Equal</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>Unassigned</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns>
          <ColumnInfo>
            <ID>4</ID>
            <FieldName>Category</FieldName>
            <DisplayName>Category</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>28</ID>
            <FieldName>TicketCreationDate</FieldName>
            <DisplayName>Created</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>29</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>Ticket Id</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>34</ID>
            <FieldName>TicketOwner</FieldName>
            <DisplayName>Owner</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>8</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>35</ID>
            <FieldName>TicketPriorityLookup</FieldName>
            <DisplayName>Priority</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>5</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>38</ID>
            <FieldName>TicketRequestor</FieldName>
            <DisplayName>Requestor</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>7</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>40</ID>
            <FieldName>TicketRequestTypeLookup</FieldName>
            <DisplayName>Request Type</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>6</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>44</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>43</ID>
            <FieldName>TicketStatus</FieldName>
            <DisplayName>Status</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>9</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text>NEW TICKETS</Text>
      <TextFontName>Segoe UI</TextFontName>
      <TextFontStyle>Regular</TextFontStyle>
      <TextFontSize>10pt</TextFontSize>
      <TextForeColor>000080</TextForeColor>
      <HideText>false</HideText>
      <Label>&amp;nbsp;&amp;nbsp;New Tickets</Label>
      <LabelFontName>Segoe UI</LabelFontName>
      <LabelFontStyle>Regular</LabelFontStyle>
      <LabelFontSize>10pt</LabelFontSize>
      <LabelForeColor>000080</LabelForeColor>
      <HideLabel>true</HideLabel>
      <ResultFontName>Segoe UI</ResultFontName>
      <ResultFontStyle>Bold</ResultFontStyle>
      <ResultFontSize>30pt</ResultFontSize>
      <ResultForeColor>000080</ResultForeColor>
      <TitlePosition>Bottom</TitlePosition>
      <FormatType>FormattedNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>230</Width>
        <Height>20</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
      <IconImage />
      <IconLocation>
        <X>0</X>
        <Y>0</Y>
      </IconLocation>
      <IconSize>
        <Width>0</Width>
        <Height>0</Height>
      </IconSize>
      <BackgroundColor>cfcfcf</BackgroundColor>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>Default Drill down</DrillDownType>
      <CustomUrl />
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <Header>New Tickets</Header>
      <Footer />
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <AdditionalInfo />
      <AdditionalFooterInfo />
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>false</IsTransparent>
      <EnableEditUrl>false</EnableEditUrl>
    </QueryFormats>
    <IsPreviewFormatted>true</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 98, 0, 0, N'SMS', N'Accent1', N'New Tickets', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.457' AS DateTime), CAST(N'2018-01-19 12:08:39.457' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (93, N'', N'Ticketing', N'Open Tickets', N'', N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>886b0b8b-412c-4bde-a100-d046ecf909ee</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>886b0b8b-412c-4bde-a100-d046ecf909ee</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>AllOpenTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>225</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>AllOpenTickets</TableName>
            <Function>Count</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>567</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>TicketId</DisplayName>
            <DataType>String</DataType>
            <TableName>AllOpenTickets</TableName>
            <Sequence>621</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses />
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>AllOpenTickets</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text>OPEN TICKETS</Text>
      <TextFontName>Segoe UI</TextFontName>
      <TextFontStyle>Regular</TextFontStyle>
      <TextFontSize>10pt</TextFontSize>
      <TextForeColor>000080</TextForeColor>
      <HideText>false</HideText>
      <Label>&amp;nbsp;&amp;nbsp;Open Tickets</Label>
      <LabelFontName>Segoe UI</LabelFontName>
      <LabelFontStyle>Regular</LabelFontStyle>
      <LabelFontSize>10pt</LabelFontSize>
      <LabelForeColor>000080</LabelForeColor>
      <HideLabel>true</HideLabel>
      <ResultFontName>Segoe UI</ResultFontName>
      <ResultFontStyle>Bold</ResultFontStyle>
      <ResultFontSize>30pt</ResultFontSize>
      <ResultForeColor>000080</ResultForeColor>
      <TitlePosition>Bottom</TitlePosition>
      <FormatType>FormattedNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>230</Width>
        <Height>20</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
      <IconImage />
      <IconLocation>
        <X>0</X>
        <Y>0</Y>
      </IconLocation>
      <IconSize>
        <Width>0</Width>
        <Height>0</Height>
      </IconSize>
      <EnableEditUrl>false</EnableEditUrl>
      <BackgroundColor>cfcfcf</BackgroundColor>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>Default Drill down</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>false</IsTransparent>
    </QueryFormats>
    <IsPreviewFormatted>true</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', N'', 2, N'', N'', 0, 0, 0, 0, 98, 0, 0, N'SMS', N'Accent1', N'Open Tickets', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.460' AS DateTime), CAST(N'2018-01-19 12:08:39.460' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (94, NULL, N'none', N'Tickets Closed this week', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>089b8c3a-2c55-45df-a0ee-0744226fb11d</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>089b8c3a-2c55-45df-a0ee-0744226fb11d</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns>
          <ColumnInfo>
            <ID>9</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>Count</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>13</ID>
            <FieldName>Id</FieldName>
            <DisplayName>Id</DisplayName>
            <DataType>none</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>0</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>TicketId</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>0</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>DashboardSummary.Closed</ColumnName>
        <DataType>Boolean</DataType>
        <Operator>Equal</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>True</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
      <WhereInfo>
        <ID>2</ID>
        <RelationOpt>AND</RelationOpt>
        <ColumnName>DashboardSummary.ClosedDate</ColumnName>
        <DataType>DateTime</DataType>
        <Operator>GreaterThanEqualTo</Operator>
        <Valuetype>Variable</Valuetype>
        <ParameterType />
        <Value>f:adddays([$Today$],-7)</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text>TICKETS CLOSED THIS WEEK</Text>
      <TextFontName>Segoe UI</TextFontName>
      <TextFontStyle>Regular</TextFontStyle>
      <TextFontSize>10pt</TextFontSize>
      <TextForeColor>000080</TextForeColor>
      <HideText>false</HideText>
      <Label />
      <LabelFontName>Segoe UI</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>8pt</LabelFontSize>
      <LabelForeColor>000080</LabelForeColor>
      <HideLabel>true</HideLabel>
      <ResultFontName>Segoe UI</ResultFontName>
      <ResultFontStyle>Bold</ResultFontStyle>
      <ResultFontSize>30pt</ResultFontSize>
      <ResultForeColor>000080</ResultForeColor>
      <TitlePosition>Bottom</TitlePosition>
      <FormatType>FormattedNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>150</Width>
        <Height>100</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
      <IconImage />
      <IconLocation>
        <X>0</X>
        <Y>0</Y>
      </IconLocation>
      <IconSize>
        <Width>0</Width>
        <Height>0</Height>
      </IconSize>
      <EnableEditUrl>false</EnableEditUrl>
      <BackgroundColor>cfcfcf</BackgroundColor>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>Default Drill down</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <Header>Tickets Closed this week</Header>
      <Footer />
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <AdditionalInfo />
      <AdditionalFooterInfo />
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>false</IsTransparent>
    </QueryFormats>
    <IsPreviewFormatted>true</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', N'', 2, N'', N'', 0, 0, 0, 0, 96, 0, 0, N'SMS', N'Accent1', N'Tickets Closed this week', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.460' AS DateTime), CAST(N'2018-01-19 12:08:39.460' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (95, NULL, N'none', N'Average Time to Close Ticket', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>ca4fd018-2b48-4e98-aa52-6f51263cbf14</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>ca4fd018-2b48-4e98-aa52-6f51263cbf14</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns>
          <ColumnInfo>
            <ID>7</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>41</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>9</ID>
            <FieldName>ClosureTAT</FieldName>
            <DisplayName>ClosureTAT</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>Avg</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>true</IsExpression>
            <Expression>DashboardSummary.ClosedDate(DateTime) - DashboardSummary.CreationDate(DateTime)</Expression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>47</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>TicketId</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>49</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>DashboardSummary.Closed</ColumnName>
        <DataType>Boolean</DataType>
        <Operator>Equal</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>True</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text>AVERAGE TIME TO CLOSE TICKET</Text>
      <TextFontName>Segoe UI</TextFontName>
      <TextFontStyle>Regular</TextFontStyle>
      <TextFontSize>10pt</TextFontSize>
      <TextForeColor>000080</TextForeColor>
      <HideText>false</HideText>
      <Label>&amp;nbsp;DAYS</Label>
      <LabelFontName>Segoe UI</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>14pt</LabelFontSize>
      <LabelForeColor>000080</LabelForeColor>
      <HideLabel>false</HideLabel>
      <ResultFontName>Segoe UI</ResultFontName>
      <ResultFontStyle>Bold</ResultFontStyle>
      <ResultFontSize>30pt</ResultFontSize>
      <ResultForeColor>000080</ResultForeColor>
      <TitlePosition>Bottom</TitlePosition>
      <FormatType>FormattedNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>170</Width>
        <Height>100</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
      <IconImage />
      <IconLocation>
        <X>0</X>
        <Y>0</Y>
      </IconLocation>
      <IconSize>
        <Width>0</Width>
        <Height>0</Height>
      </IconSize>
      <EnableEditUrl>false</EnableEditUrl>
      <BackgroundColor>cfcfcf</BackgroundColor>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>None</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>false</IsTransparent>
    </QueryFormats>
    <IsPreviewFormatted>true</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', N'', 2, N'', N'', 1, 0, 0, 0, 99, 0, 0, N'SMS', N'Accent1', N'Average Time to Close Ticket', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.460' AS DateTime), CAST(N'2018-01-19 12:08:39.460' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (96, NULL, N'none', N'Average Time to respond to New Tickets', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>9f9b1967-6e20-40cb-bb07-e83527924d3c</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>9f9b1967-6e20-40cb-bb07-e83527924d3c</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns>
          <ColumnInfo>
            <ID>6</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>41</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>37</ID>
            <FieldName>ResourceTAT</FieldName>
            <DisplayName>ResourceTAT</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>Avg</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>true</IsExpression>
            <Expression>DashboardSummary.CreationDate(DateTime) - DashboardSummary.ActualHours(Double)</Expression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>47</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>TicketId</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>47</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>DashboardSummary.Closed</ColumnName>
        <DataType>Boolean</DataType>
        <Operator>Equal</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>True</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text>AVERAGE TIME TO RESPOND TO NEW TICKETS</Text>
      <TextFontName>Segoe UI</TextFontName>
      <TextFontStyle>Regular</TextFontStyle>
      <TextFontSize>10pt</TextFontSize>
      <TextForeColor>000080</TextForeColor>
      <HideText>false</HideText>
      <Label>&amp;nbsp;DAYS</Label>
      <LabelFontName>Segoe UI</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>14pt</LabelFontSize>
      <LabelForeColor>000080</LabelForeColor>
      <HideLabel>false</HideLabel>
      <ResultFontName>Segoe UI</ResultFontName>
      <ResultFontStyle>Bold</ResultFontStyle>
      <ResultFontSize>30pt</ResultFontSize>
      <ResultForeColor>000080</ResultForeColor>
      <TitlePosition>Bottom</TitlePosition>
      <FormatType>FormattedNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>170</Width>
        <Height>100</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
      <IconImage />
      <IconLocation>
        <X>0</X>
        <Y>0</Y>
      </IconLocation>
      <IconSize>
        <Width>0</Width>
        <Height>0</Height>
      </IconSize>
      <EnableEditUrl>false</EnableEditUrl>
      <BackgroundColor>cfcfcf</BackgroundColor>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>None</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>false</IsTransparent>
    </QueryFormats>
    <IsPreviewFormatted>true</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', N'', 2, N'', N'', 0, 0, 0, 0, 102, 0, 0, N'SMS', N'Accent1', N'Average Time to respond to New Tickets', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.460' AS DateTime), CAST(N'2018-01-19 12:08:39.460' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (97, NULL, N'Ticketing', N'Assets', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>f93be384-15d1-448d-b33b-e3238e66f5de</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>f93be384-15d1-448d-b33b-e3238e66f5de</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>CMDB-OpenTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>37</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>CMDB-OpenTickets</TableName>
            <Sequence>120</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>112</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>TicketId</DisplayName>
            <DataType>String</DataType>
            <TableName>CMDB-OpenTickets</TableName>
            <Sequence>119</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses />
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>CMDB-OpenTickets</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', N'', 2, N'', N'', 0, 0, 0, 0, 103, 0, 0, N'SMS', N'Accent1', N'Assets', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.460' AS DateTime), CAST(N'2018-01-19 12:08:39.460' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (98, N'', N'none', N'NPRs Pending Approval', N'', N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>3a34228f-5874-44d6-9fb5-64db73303678</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>3a34228f-5874-44d6-9fb5-64db73303678</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy>
      <OrderByInfo>
        <Num>1</Num>
        <Column>
          <ID>205</ID>
          <FieldName>Title</FieldName>
          <DisplayName>Title</DisplayName>
          <DataType>String</DataType>
          <TableName>NPRRequest</TableName>
          <Function>none</Function>
          <Sequence>2</Sequence>
          <Selected>true</Selected>
          <Hidden>false</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
        </Column>
        <orderBy>ASC</orderBy>
      </OrderByInfo>
    </OrderBy>
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>NPRRequest</Name>
        <Columns>
          <ColumnInfo>
            <ID>48</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>NPRRequest</TableName>
            <Sequence>212</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>16</ID>
            <FieldName>Created</FieldName>
            <DisplayName>Created</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>NPRRequest</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>75</ID>
            <FieldName>ProjectCost</FieldName>
            <DisplayName>Project Cost</DisplayName>
            <DataType>Double</DataType>
            <TableName>NPRRequest</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>80</ID>
            <FieldName>ProjectInitiativeLookup</FieldName>
            <DisplayName>Project Initiative Lookup</DisplayName>
            <DataType>String</DataType>
            <TableName>NPRRequest</TableName>
            <Function>none</Function>
            <Sequence>5</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>81</ID>
            <FieldName>ProjectRank</FieldName>
            <DisplayName>Project Rank</DisplayName>
            <DataType>String</DataType>
            <TableName>NPRRequest</TableName>
            <Function>none</Function>
            <Sequence>6</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>93</ID>
            <FieldName>TicketApprovedRFEAmount</FieldName>
            <DisplayName>Approved RFE Amount</DisplayName>
            <DataType>Double</DataType>
            <TableName>NPRRequest</TableName>
            <Function>none</Function>
            <Sequence>7</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>126</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>NPR Id</DisplayName>
            <DataType>String</DataType>
            <TableName>NPRRequest</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>130</ID>
            <FieldName>TicketInitiator</FieldName>
            <DisplayName>Initiator</DisplayName>
            <DataType>String</DataType>
            <TableName>NPRRequest</TableName>
            <Function>none</Function>
            <Sequence>8</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>180</ID>
            <FieldName>TicketStatus</FieldName>
            <DisplayName>Status</DisplayName>
            <DataType>String</DataType>
            <TableName>NPRRequest</TableName>
            <Function>none</Function>
            <Sequence>9</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>205</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>NPRRequest</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>NPRRequest.StageStep</ColumnName>
        <DataType>Double</DataType>
        <Operator>LessThan</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>6</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals>
      <ColumnInfo>
        <ID>6</ID>
        <FieldName>TicketApprovedRFEAmount</FieldName>
        <DisplayName>Approved RFE Amount</DisplayName>
        <DataType>Double</DataType>
        <TableName>NPRRequest</TableName>
        <Function>Sum</Function>
        <Sequence>7</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
      <ColumnInfo>
        <ID>7</ID>
        <FieldName>TicketId</FieldName>
        <DisplayName>NPR Id</DisplayName>
        <DataType>String</DataType>
        <TableName>NPRRequest</TableName>
        <Function>Count</Function>
        <Sequence>1</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
    </Totals>
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>NPRRequest</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', N'', 2, N'', N'', 0, 0, 0, 0, 104, 0, 0, N'SMS', N'Accent1', N'NPRs Pending Approval', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.460' AS DateTime), CAST(N'2018-01-19 12:08:39.460' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (99, NULL, N'none', N'NPR Portfolio $$', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>6dabeb09-dcff-4113-9143-ad9e0bb2ebd7</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>6dabeb09-dcff-4113-9143-ad9e0bb2ebd7</QueryId>
  <QueryTable />
  <QueryInfo>
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns>
          <ColumnInfo>
            <ID>6</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>40</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>5</ID>
            <FieldName>Category</FieldName>
            <DisplayName>Category</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>14</ID>
            <FieldName>InitiatedDate</FieldName>
            <DisplayName>Initiated Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>46</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>Ticket Id</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses />
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text />
      <TextFontName>Times New Roman</TextFontName>
      <TextFontStyle>Bold</TextFontStyle>
      <TextFontSize>8pt</TextFontSize>
      <TextForeColor>000000</TextForeColor>
      <HideText>false</HideText>
      <Label />
      <LabelFontName>Times New Roman</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>8pt</LabelFontSize>
      <LabelForeColor>000000</LabelForeColor>
      <HideLabel>false</HideLabel>
      <ResultFontName>Times New Roman</ResultFontName>
      <ResultFontStyle>Bold</ResultFontStyle>
      <ResultFontSize>8pt</ResultFontSize>
      <ResultForeColor>000000</ResultForeColor>
      <TitlePosition>Top</TitlePosition>
      <FormatType>SimpleNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>0</Width>
        <Height>0</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
      <IconImage />
      <IconLocation>
        <X>0</X>
        <Y>0</Y>
      </IconLocation>
      <IconSize>
        <Width>0</Width>
        <Height>0</Height>
      </IconSize>
      <EnableEditUrl>false</EnableEditUrl>
      <BackgroundColor>ffffff</BackgroundColor>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>None</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>false</IsTransparent>
    </QueryFormats>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', N'', 2, N'', N'', 0, 0, 0, 0, 105, 0, 0, N'SMS', N'Accent1', N'NPR Portfolio $$', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.460' AS DateTime), CAST(N'2018-01-19 12:08:39.460' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (100, NULL, N'Ticketing', N'My Closed Tickets', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>089b8c3a-2c55-45df-a0ee-0744226fb11d</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>089b8c3a-2c55-45df-a0ee-0744226fb11d</QueryId>
  <QueryTable />
  <QueryInfo>
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>AllClosedTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>94</ID>
            <FieldName>Closed</FieldName>
            <DisplayName>Closed</DisplayName>
            <DataType>Boolean</DataType>
            <TableName>AllClosedTickets</TableName>
            <Function>Count</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>225</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>AllClosedTickets</TableName>
            <Sequence>225</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>567</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>TicketId</DisplayName>
            <DataType>String</DataType>
            <TableName>AllClosedTickets</TableName>
            <Sequence>567</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>AllClosedTickets.Requestor</ColumnName>
        <DataType>String</DataType>
        <Operator>Equal</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>[$me$]</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>AllClosedTickets</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text>My Closed Tickets</Text>
      <TextFontName>Segoe UI</TextFontName>
      <TextFontStyle>Regular</TextFontStyle>
      <TextFontSize>10pt</TextFontSize>
      <TextForeColor>ffffff</TextForeColor>
      <HideText>false</HideText>
      <Label />
      <LabelFontName>Segoe UI</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>8pt</LabelFontSize>
      <LabelForeColor>ffffff</LabelForeColor>
      <HideLabel>true</HideLabel>
      <ResultFontName>Segoe UI</ResultFontName>
      <ResultFontStyle>Bold</ResultFontStyle>
      <ResultFontSize>30pt</ResultFontSize>
      <ResultForeColor>ffffff</ResultForeColor>
      <TitlePosition>Bottom</TitlePosition>
      <FormatType>FormattedNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>150</Width>
        <Height>100</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
      <IconImage />
      <IconLocation>
        <X>0</X>
        <Y>0</Y>
      </IconLocation>
      <IconSize>
        <Width>0</Width>
        <Height>0</Height>
      </IconSize>
      <EnableEditUrl>false</EnableEditUrl>
      <BackgroundColor>003366</BackgroundColor>
      <TextAlign>center</TextAlign>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>Default Drill down</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <Header>My Closed Tickets</Header>
      <Footer />
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <AdditionalInfo />
      <AdditionalFooterInfo />
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>true</IsTransparent>
    </QueryFormats>
    <IsPreviewFormatted>true</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', N'', 2, N'', N'', 0, 0, 0, 0, 105, 0, 0, N'SMS', N'Accent1', N'My Closed Tickets', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.463' AS DateTime), CAST(N'2018-01-19 12:08:39.463' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (101, NULL, NULL, N'My Open Tickets', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery
	xmlns:xsd="http://www.w3.org/2001/XMLSchema"
	xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<type>Panel</type>
	<ContainerTitle />
	<Description />
	<CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
	<Order>0</Order>
	<DashboardID>6cb645c6-af56-4ea5-ab89-ba7f7a131c0d</DashboardID>
	<HideZoomView>false</HideZoomView>
	<HideTableView>false</HideTableView>
	<HidewDownloadView>false</HidewDownloadView>
	<StartFromNewLine>false</StartFromNewLine>
	<Id />
	<QueryId>6cb645c6-af56-4ea5-ab89-ba7f7a131c0d</QueryId>
	<QueryTable />
	<QueryInfo>
		<JoinList>
			<Joins>
				<JoinType>INNER</JoinType>
				<FirstTable>ModuleUserStatistics</FirstTable>
				<SecondTable>Modules</SecondTable>
				<OperatorType>=</OperatorType>
				<FirstColumn>ModuleUserStatistics.ModuleId</FirstColumn>
				<SecondColumn>Modules.ModuleId</SecondColumn>
				<DataTypeFirstCol>Double</DataTypeFirstCol>
				<DataTypeSecondCol>Double</DataTypeSecondCol>
			</Joins>
		</JoinList>
		<GroupBy />
		<OrderBy />
		<Tables>
			<TableInfo>
				<ID>1</ID>
				<Name>ModuleUserStatistics</Name>
				<Columns>
					<ColumnInfo>
						<ID>0</ID>
						<FieldName>ID</FieldName>
						<DisplayName>ID</DisplayName>
						<DataType>Integer</DataType>
						<TableName>ModuleUserStatistics</TableName>
						<Sequence>0</Sequence>
						<Selected>false</Selected>
						<Hidden>true</Hidden>
						<IsExpression>false</IsExpression>
						<IsDrillDown>false</IsDrillDown>
						<IsFormattedColumn>false</IsFormattedColumn>
					</ColumnInfo>
					<ColumnInfo>
						<ID>13</ID>
						<FieldName>TicketId</FieldName>
						<DisplayName>Ticket ID</DisplayName>
						<DataType>String</DataType>
						<TableName>ModuleUserStatistics</TableName>
						<Function>DistinctCount</Function>
						<Sequence>1</Sequence>
						<Selected>true</Selected>
						<Hidden>false</Hidden>
						<IsExpression>false</IsExpression>
						<IsDrillDown>false</IsDrillDown>
						<IsFormattedColumn>false</IsFormattedColumn>
					</ColumnInfo>
				</Columns>
			</TableInfo>
			<TableInfo>
				<ID>2</ID>
				<Name>Config_Modules</Name>
				<Columns>
					<ColumnInfo>
						<ID>0</ID>
						<FieldName>ID</FieldName>
						<DisplayName>ID</DisplayName>
						<DataType>Integer</DataType>
						<TableName>Config_Modules</TableName>
						<Sequence>0</Sequence>
						<Selected>false</Selected>
						<Hidden>true</Hidden>
						<IsExpression>false</IsExpression>
						<IsDrillDown>false</IsDrillDown>
						<IsFormattedColumn>false</IsFormattedColumn>
					</ColumnInfo>
				</Columns>
			</TableInfo>
		</Tables>
		<WhereClauses>
			<WhereInfo>
				<ID>2</ID>
				<RelationOpt>AND</RelationOpt>
				<ColumnName>Modules.ModuleType</ColumnName>
				<DataType>String</DataType>
				<Operator>Equal</Operator>
				<Valuetype>Constant</Valuetype>
				<ParameterType />
				<Value>SMS</Value>
				<ParameterName />
				<ParameterRequired>false</ParameterRequired>
			</WhereInfo>
			<WhereInfo>
				<ID>2</ID>
				<RelationOpt>AND</RelationOpt>
				<ColumnName>ModuleUserStatistics.TicketUser</ColumnName>
				<DataType>String</DataType>
				<Operator>Equal</Operator>
				<Valuetype>Constant</Valuetype>
				<ParameterType />
				<Value>[$me$]</Value>
				<ParameterName />
				<ParameterRequired>false</ParameterRequired>
			</WhereInfo>
		</WhereClauses>
		<Totals />
		<DrillDownTables>
			<TableInfo>
				<ID>1</ID>
				<Name>ModuleUserStatistics</Name>
				<Columns />
			</TableInfo>
			<TableInfo>
				<ID>2</ID>
				<Name>Config_Modules</Name>
				<Columns />
			</TableInfo>
		</DrillDownTables>
		<QueryFormats>
			<Text>My Open Tickets</Text>
			<TextFontName>Segoe UI</TextFontName>
			<TextFontStyle>Regular</TextFontStyle>
			<TextFontSize>10pt</TextFontSize>
			<TextForeColor>ffffff</TextForeColor>
			<HideText>false</HideText>
			<Label />
			<LabelFontName>Segoe UI</LabelFontName>
			<LabelFontStyle>Bold</LabelFontStyle>
			<LabelFontSize>8pt</LabelFontSize>
			<LabelForeColor>ffffff</LabelForeColor>
			<HideLabel>false</HideLabel>
			<ResultFontName>Segoe UI</ResultFontName>
			<ResultFontStyle>Bold</ResultFontStyle>
			<ResultFontSize>30pt</ResultFontSize>
			<ResultForeColor>ffffff</ResultForeColor>
			<TitlePosition>Bottom</TitlePosition>
			<FormatType>FormattedNumber</FormatType>
			<BackgroundImage />
			<SizeOfFrame>
				<Width>150</Width>
				<Height>100</Height>
			</SizeOfFrame>
			<Location>
				<X>0</X>
				<Y>0</Y>
			</Location>
			<ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
			<IconImage />
			<IconLocation>
				<X>0</X>
				<Y>0</Y>
			</IconLocation>
			<IconSize>
				<Width>0</Width>
				<Height>0</Height>
			</IconSize>
			<BackgroundColor />
			<TextAlign>center</TextAlign>
			<BorderColor>ffffff</BorderColor>
			<BorderWidth>0</BorderWidth>
			<HeaderColor>ffffff</HeaderColor>
			<RowColor>ffffff</RowColor>
			<RowAlternateColor>ffffff</RowAlternateColor>
			<DrillDownType>Default Drill down</DrillDownType>
			<NavigateType>Popup</NavigateType>
			<QueryId>21</QueryId>
			<ShowCompanyLogo>false</ShowCompanyLogo>
			<ShowDateInFooter>false</ShowDateInFooter>
			<IsTransparent>true</IsTransparent>
			<EnableEditUrl>false</EnableEditUrl>
		</QueryFormats>
		<IsPreviewFormatted>true</IsPreviewFormatted>
		<IsGroupByExpanded>false</IsGroupByExpanded>
		<ParameterList />
	</QueryInfo>
	<ScheduleActionValue>
		<ScheduleId>0</ScheduleId>
		<StartTime>0001-01-01T00:00:00</StartTime>
		<ActionType>Email</ActionType>
		<Recurring>false</Recurring>
		<RecurringInterval xsi:nil="true" />
		<RecurringEndDate xsi:nil="true" />
		<IsEnable>false</IsEnable>
	</ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 108, 200, 150, N'SMS', N'Accent1', N'My Open Tickets', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.463' AS DateTime), CAST(N'2018-01-19 12:08:39.463' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (102, NULL, NULL, N'My Open Department', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>cc421995-fb7d-41a5-91f0-a9ae12e8b981</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>cc421995-fb7d-41a5-91f0-a9ae12e8b981</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>AllOpenSMSTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>6</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>AllOpenSMSTickets</TableName>
            <Sequence>31</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>16</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>Ticket Id</DisplayName>
            <DataType>String</DataType>
            <TableName>AllOpenSMSTickets</TableName>
            <Function>Count</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>AllOpenSMSTickets.DepartmentLookup</ColumnName>
        <DataType>String</DataType>
        <Operator>like</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>[$mydepartment$]</Value>
        <ParameterName />
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>AllOpenSMSTickets</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text>Department Open Tickets</Text>
      <TextFontName>Segoe UI</TextFontName>
      <TextFontStyle>Regular</TextFontStyle>
      <TextFontSize>10pt</TextFontSize>
      <TextForeColor>ffffff</TextForeColor>
      <HideText>false</HideText>
      <Label />
      <LabelFontName>Segoe UI</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>8pt</LabelFontSize>
      <LabelForeColor>ffffff</LabelForeColor>
      <HideLabel>false</HideLabel>
      <ResultFontName>Segoe UI</ResultFontName>
      <ResultFontStyle>Bold</ResultFontStyle>
      <ResultFontSize>30pt</ResultFontSize>
      <ResultForeColor>ffffff</ResultForeColor>
      <TitlePosition>Bottom</TitlePosition>
      <FormatType>FormattedNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>150</Width>
        <Height>100</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
      <IconImage />
      <IconLocation>
        <X>0</X>
        <Y>0</Y>
      </IconLocation>
      <IconSize>
        <Width>0</Width>
        <Height>0</Height>
      </IconSize>
      <BackgroundColor/>
      <TextAlign>center</TextAlign>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>Default Drill down</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <IsTransparent>true</IsTransparent>
    </QueryFormats>
    <IsPreviewFormatted>true</IsPreviewFormatted>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 109, 0, 0, N'SMS', N'Accent1', N'My Open Department', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.463' AS DateTime), CAST(N'2018-01-19 12:08:39.463' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (103, NULL, N'Ticketing', N'My Closed Departments', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>19091d1d-e935-4ff0-ac8a-6bcb5f2acdf6</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>19091d1d-e935-4ff0-ac8a-6bcb5f2acdf6</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>AllClosedTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>16</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>AllClosedTickets</TableName>
            <Sequence>69</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>44</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>Ticket Id</DisplayName>
            <DataType>String</DataType>
            <TableName>AllClosedTickets</TableName>
            <Function>Count</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>AllClosedTickets.DepartmentLookup</ColumnName>
        <DataType>String</DataType>
        <Operator>like</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>[$mydepartment$]</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>AllClosedTickets</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text>Department Closed Tickets</Text>
      <TextFontName>Segoe UI</TextFontName>
      <TextFontStyle>Regular</TextFontStyle>
      <TextFontSize>10pt</TextFontSize>
      <TextForeColor>ffffff</TextForeColor>
      <HideText>false</HideText>
      <Label />
      <LabelFontName>Segoe UI</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>8pt</LabelFontSize>
      <LabelForeColor>ffffff</LabelForeColor>
      <HideLabel>false</HideLabel>
      <ResultFontName>Segoe UI</ResultFontName>
      <ResultFontStyle>Bold</ResultFontStyle>
      <ResultFontSize>30pt</ResultFontSize>
      <ResultForeColor>ffffff</ResultForeColor>
      <TitlePosition>Bottom</TitlePosition>
      <FormatType>FormattedNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>150</Width>
        <Height>100</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
      <IconImage />
      <IconLocation>
        <X>0</X>
        <Y>0</Y>
      </IconLocation>
      <IconSize>
        <Width>0</Width>
        <Height>0</Height>
      </IconSize>
      <BackgroundColor />
      <TextAlign>center</TextAlign>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>Default Drill down</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>true</IsTransparent>
    </QueryFormats>
    <IsPreviewFormatted>true</IsPreviewFormatted>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 110, 200, 150, N'SMS', N'Accent1', N'My Closed Departments', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.467' AS DateTime), CAST(N'2018-01-19 12:08:39.467' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (104, NULL, N'Project Management', N'PMMProjectDetail', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>48a597b7-9145-4788-b5f8-21e9813af8e6</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>48a597b7-9145-4788-b5f8-21e9813af8e6</QueryId>
  <QueryTable>PMMProjects</QueryTable>
  <QueryInfo>
    <GroupBy />
    <OrderBy>
      <OrderByInfo>
        <Num>1</Num>
        <Column>
          <ID>100</ID>
          <FieldName>TicketId</FieldName>
          <DisplayName>Ticket ID</DisplayName>
          <DataType>String</DataType>
          <TableName>PMMProjects</TableName>
          <Function>none</Function>
          <Sequence>1</Sequence>
          <Selected>true</Selected>
          <Hidden>false</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
          <Alignment>None</Alignment>
        </Column>
        <orderBy>ASC</orderBy>
      </OrderByInfo>
    </OrderBy>
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>PMMProjects</Name>
        <Columns>
          <ColumnInfo>
            <ID>44</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>PMMProjects</TableName>
            <Sequence>147</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>80</ID>
            <FieldName>TicketActualCompletionDate</FieldName>
            <DisplayName>Due Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>11</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>100</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>Ticket ID</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>112</ID>
            <FieldName>TicketPctComplete</FieldName>
            <DisplayName>% Complete</DisplayName>
            <DataType>Percent*100</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>12</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>113</ID>
            <FieldName>TicketPriorityLookup</FieldName>
            <DisplayName>Priority</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>5</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>116</ID>
            <FieldName>TicketProjectManager</FieldName>
            <DisplayName>Project Mgr(s)</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>10</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>132</ID>
            <FieldName>TicketStatus</FieldName>
            <DisplayName>Status</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>7</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>142</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>65</ID>
            <FieldName>ProjectRank</FieldName>
            <DisplayName>Rank</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>6</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>123</ID>
            <FieldName>TicketRequestTypeLookup</FieldName>
            <DisplayName>Project Type</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>8</ID>
            <FieldName>CompanyMultiLookup</FieldName>
            <DisplayName>Company</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>8</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>36</ID>
            <FieldName>DivisionMultiLookup</FieldName>
            <DisplayName>Division(s)</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>9</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>59</ID>
            <FieldName>ProjectCost</FieldName>
            <DisplayName>Spend To Date</DisplayName>
            <DataType>Currency</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>14</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>138</ID>
            <FieldName>TicketTotalCost</FieldName>
            <DisplayName>Budget Amount</DisplayName>
            <DataType>Currency</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>13</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>82</ID>
            <FieldName>TicketApprovedRFE</FieldName>
            <DisplayName>Project Code</DisplayName>
            <DataType>String</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>146</ID>
            <FieldName>ProjectHealth</FieldName>
            <DisplayName>ProjectHealth</DisplayName>
            <DataType>none</DataType>
            <TableName>PMMProjects</TableName>
            <Function>none</Function>
            <Sequence>15</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>PMMProjects.TicketClosed</ColumnName>
        <DataType>String</DataType>
        <Operator>NotEqual</Operator>
        <Valuetype>Constant</Valuetype>
        <Value>1</Value>
        <ParameterName />
        <ParameterRequired>false</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals>
      <ColumnInfo>
        <ID>3</ID>
        <FieldName>TicketId</FieldName>
        <DisplayName>Ticket ID</DisplayName>
        <DataType>String</DataType>
        <TableName>PMMProjects</TableName>
        <Function>Count</Function>
        <Sequence>1</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
      <ColumnInfo>
        <ID>13</ID>
        <FieldName>ProjectCost</FieldName>
        <DisplayName>Spend To Date</DisplayName>
        <DataType>Currency</DataType>
        <TableName>PMMProjects</TableName>
        <Function>Sum</Function>
        <Sequence>14</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
      <ColumnInfo>
        <ID>14</ID>
        <FieldName>TicketTotalCost</FieldName>
        <DisplayName>Budget Amount</DisplayName>
        <DataType>Currency</DataType>
        <TableName>PMMProjects</TableName>
        <Function>Sum</Function>
        <Sequence>13</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
    </Totals>
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>PMMProjects</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text />
      <TextFontName>Times New Roman</TextFontName>
      <TextFontStyle>Bold</TextFontStyle>
      <TextFontSize>8pt</TextFontSize>
      <TextForeColor>000000</TextForeColor>
      <HideText>false</HideText>
      <Label />
      <LabelFontName>Times New Roman</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>8pt</LabelFontSize>
      <LabelForeColor>000000</LabelForeColor>
      <HideLabel>false</HideLabel>
      <ResultFontName>Times New Roman</ResultFontName>
      <ResultFontStyle>Bold</ResultFontStyle>
      <ResultFontSize>8pt</ResultFontSize>
      <ResultForeColor>000000</ResultForeColor>
      <TitlePosition>Top</TitlePosition>
      <FormatType>Table</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>0</Width>
        <Height>0</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithoutIconOrBorder</ResultPanelDesign>
      <IconImage />
      <IconLocation>
        <X>0</X>
        <Y>0</Y>
      </IconLocation>
      <IconSize>
        <Width>0</Width>
        <Height>0</Height>
      </IconSize>
      <BackgroundColor>ffffff</BackgroundColor>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>None</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>false</IsTransparent>
    </QueryFormats>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Email</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 98, 0, 0, N'Project Listing', N'Accent1', N'PMM Project Details Demo Data', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.467' AS DateTime), CAST(N'2018-01-19 12:08:39.467' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
GO
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (105, NULL, NULL, N'Wikis', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>96efc1a7-19f6-4858-8147-56088a2911ee</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>96efc1a7-19f6-4858-8147-56088a2911ee</QueryId>
  <QueryTable>WikiArticles</QueryTable>
  <QueryInfo>
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>WikiArticles</Name>
        <Columns>
          <ColumnInfo>
            <ID>0</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>WikiArticles</TableName>
            <Sequence>0</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses />
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>WikiArticles</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Alert</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 112, 0, 0, N'SMS', N'Accent1', N'Wikis', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.570' AS DateTime), CAST(N'2018-01-19 12:08:39.570' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (106, NULL, N'', N'Asset Summary', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Asset Summary</ContainerTitle>
  <Description>Asset Summary</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>ef09dff6-f7bf-40a7-b365-862df6077f82</DashboardID>
  <HideZoomView>true</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>34431450-e497-4af9-9999-638207ecc0d9</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Asset Types</Title>
      <SelectedField>RequestTypeLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>1</OrderByExpression>
      <OrderBy>descending</OrderBy>
      <EnableSorting>true</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>None</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>sideBySideFullStackedBarValAsPertge</Key>
          <Value xsi:type="xsd:string">False</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title># of Assets</Title>
      <FactTable />
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle />
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>CMDB-OpenTickets</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>LeftOutside</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>TopToBottom</Direction>
  <MaxHorizontalPercentage>50</MaxHorizontalPercentage>
  <MaxVerticalPercentage>50</MaxVerticalPercentage>
  <Palette>Northern Lights</Palette>
  <BGColor>#E7EAFE</BGColor>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 1, 240, 680, N'', N'Accent1', N'Asset Summary', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.570' AS DateTime), CAST(N'2018-01-19 12:08:39.570' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (107, NULL, N'', N'Assets By Location', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Assets By Location</ContainerTitle>
  <Description>Assets By Location</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>29394ffa-c44b-4a7c-9974-fa582dcaa20f</DashboardID>
  <HideZoomView>true</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>cba92ba0-88f6-4cb9-8280-1eb0c45c0022</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>By Location</Title>
      <SelectedField>LocationLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>1</OrderByExpression>
      <OrderBy>descending</OrderBy>
      <EnableSorting>true</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>sideBySideFullStackedBarValAsPertge</Key>
          <Value xsi:type="xsd:string">False</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title># of Assets</Title>
      <FactTable />
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle />
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>CMDB-OpenTickets</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Pastel Kit</Palette>
  <BGColor>#E7EAFE</BGColor>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 1, 300, 280, N'', N'Accent1', N'Assets By Location', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.570' AS DateTime), CAST(N'2018-01-19 12:08:39.570' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (108, NULL, NULL, N'SLA Summary', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>8f13ed05-b742-4d33-92b3-f6483d05ca1e</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>8f13ed05-b742-4d33-92b3-f6483d05ca1e</QueryId>
  <QueryTable />
  <QueryInfo>
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns>
          <ColumnInfo>
            <ID>9</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>48</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>3</ID>
            <FieldName>AssignmentSLAMet</FieldName>
            <DisplayName>Assignment SLA Met</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>19</ID>
            <FieldName>ResolutionSLAMet</FieldName>
            <DisplayName>Resolution SLA Met</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>29</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>Ticket Id</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>43</ID>
            <FieldName>TicketStatus</FieldName>
            <DisplayName>Status</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>44</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>none</Function>
            <Sequence>5</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>DashboardSummary.ModuleNameLookup</ColumnName>
        <DataType>String</DataType>
        <Operator>Equal</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>TSR</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
      <WhereInfo>
        <ID>2</ID>
        <RelationOpt>AND</RelationOpt>
        <ColumnName>DashboardSummary.ALLSLAsMet</ColumnName>
        <DataType>String</DataType>
        <Operator>NotEqual</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>1</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
      <WhereInfo>
        <ID>3</ID>
        <RelationOpt>AND</RelationOpt>
        <ColumnName>DashboardSummary.TicketClosed</ColumnName>
        <DataType>Boolean</DataType>
        <Operator>Equal</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>True</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Alert</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 112, 0, 0, NULL, N'Accent1', N'SLA Summary', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.570' AS DateTime), CAST(N'2018-01-19 12:08:39.570' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (109, NULL, NULL, N'Tickets over SLA', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>34538beb-1735-45fd-9fce-c6bc1765a6ba</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>34538beb-1735-45fd-9fce-c6bc1765a6ba</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy>
      <GroupByInfo>
        <Column>
          <ID>26</ID>
          <FieldName>NextSLATime</FieldName>
          <DisplayName>Date SLA Exceeded</DisplayName>
          <DataType>DateTime</DataType>
          <TableName>AllOpenTickets</TableName>
          <Function>none</Function>
          <Sequence>1</Sequence>
          <Selected>true</Selected>
          <Hidden>false</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
          <Alignment>None</Alignment>
        </Column>
        <Num>1</Num>
      </GroupByInfo>
    </GroupBy>
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>AllOpenTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>19</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>AllOpenTickets</TableName>
            <Sequence>75</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>26</ID>
            <FieldName>NextSLATime</FieldName>
            <DisplayName>Date SLA Exceeded</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>AllOpenTickets</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>50</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>Ticket Id</DisplayName>
            <DataType>String</DataType>
            <TableName>AllOpenTickets</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>70</ID>
            <FieldName>TicketStatus</FieldName>
            <DisplayName>Ticket Status</DisplayName>
            <DataType>String</DataType>
            <TableName>AllOpenTickets</TableName>
            <Function>none</Function>
            <Sequence>5</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>74</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>AllOpenTickets</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>61</ID>
            <FieldName>TicketPRP</FieldName>
            <DisplayName>Ticket PRP</DisplayName>
            <DataType>String</DataType>
            <TableName>AllOpenTickets</TableName>
            <Function>none</Function>
            <Sequence>6</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>63</ID>
            <FieldName>TicketRequestor</FieldName>
            <DisplayName>Ticket Requestor</DisplayName>
            <DataType>String</DataType>
            <TableName>AllOpenTickets</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>34</ID>
            <FieldName>PRPGroup</FieldName>
            <DisplayName>PRP Group</DisplayName>
            <DataType>String</DataType>
            <TableName>AllOpenTickets</TableName>
            <Function>none</Function>
            <Sequence>7</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>55</ID>
            <FieldName>TicketOwner</FieldName>
            <DisplayName>Ticket Owner</DisplayName>
            <DataType>String</DataType>
            <TableName>AllOpenTickets</TableName>
            <Function>none</Function>
            <Sequence>8</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>64</ID>
            <FieldName>TicketRequestTypeCategory</FieldName>
            <DisplayName>Ticket Request Type Category</DisplayName>
            <DataType>String</DataType>
            <TableName>AllOpenTickets</TableName>
            <Function>none</Function>
            <Sequence>9</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>65</ID>
            <FieldName>TicketRequestTypeLookup</FieldName>
            <DisplayName>Ticket Request Type Lookup</DisplayName>
            <DataType>String</DataType>
            <TableName>AllOpenTickets</TableName>
            <Function>none</Function>
            <Sequence>10</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>AllOpenTickets.NextSLATime</ColumnName>
        <DataType>DateTime</DataType>
        <Operator>LessThanEqualTo</Operator>
        <Valuetype>Variable</Valuetype>
        <ParameterType />
        <Value>f:adddays([$Today$],0)</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals>
      <ColumnInfo>
        <ID>3</ID>
        <FieldName>TicketId</FieldName>
        <DisplayName>Ticket Id</DisplayName>
        <DataType>String</DataType>
        <TableName>AllOpenTickets</TableName>
        <Function>Count</Function>
        <Sequence>2</Sequence>
        <Selected>true</Selected>
        <Hidden>false</Hidden>
        <IsExpression>false</IsExpression>
        <IsDrillDown>false</IsDrillDown>
        <IsFormattedColumn>false</IsFormattedColumn>
      </ColumnInfo>
    </Totals>
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>AllOpenTickets</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Alert</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 114, 0, 0, NULL, N'Accent1', N'Tickets over SLA', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.570' AS DateTime), CAST(N'2018-01-19 12:08:39.570' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (110, NULL, NULL, N'Tickets over SLA Chart', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Tickets over SLA Chart</ContainerTitle>
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>057cf84a-5d11-4278-9f4e-4bfc1d81413d</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>bb804969-2aaf-4cfd-9d29-d9825a2ba617</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title />
      <SelectedField />
      <Sequence>0</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>None</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
    </ChartDimension>
  </Dimensions>
  <Expressions />
  <FactTable>AllOpenTickets</FactTable>
  <BasicDateFitlerStartField>NextSLATime</BasicDateFitlerStartField>
  <BasicDateFitlerEndField>NextSLATime</BasicDateFitlerEndField>
  <BasicDateFilterDefaultView />
  <BasicFilter>[NextSLATime] &lt; #06/07/2017#</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 3, 300, 300, NULL, N'Accent1', N'Tickets over SLA Chart', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.570' AS DateTime), CAST(N'2018-01-19 12:08:39.570' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (111, NULL, NULL, N'Service Counts by Category', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Service Counts by Category</ContainerTitle>
  <Description>Show # of configured services by category</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>e097c248-a669-4559-a499-cb85217c34cf</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#E7EAFE</BGColor>
  <Id />
  <ChartId>7d648dfe-ad97-407b-98c6-146b2aa66b9c</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Category</Title>
      <SelectedField>ServiceCategoryNameLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>sideBySideFullStackedBarValAsPertge</Key>
          <Value xsi:type="xsd:string">True</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title>Count</Title>
      <FactTable />
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle />
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>Services</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter>[IsActivated] = ''1'' AND [ServiceCategoryNameLookup] &lt;&gt; ''~ModuleAgent~'' AND [ServiceCategoryNameLookup] &lt;&gt; ''~ModuleFeedback~''</BasicFilter>
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, 1, NULL, 4, 480, 360, NULL, N'Accent1', N'Service Counts by Category', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.570' AS DateTime), CAST(N'2018-01-19 12:08:39.570' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (112, NULL, NULL, N'Service Counts', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>a03bed22-eebb-4608-b2ae-46790609970e</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>a03bed22-eebb-4608-b2ae-46790609970e</QueryId>
  <QueryTable>Services</QueryTable>
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy>
      <OrderByInfo>
        <Num>1</Num>
        <Column>
          <ID>31</ID>
          <FieldName>ServiceCategoryNameLookup</FieldName>
          <DisplayName>Category Name</DisplayName>
          <DataType>String</DataType>
          <TableName>Services</TableName>
          <Function>none</Function>
          <Sequence>1</Sequence>
          <Selected>true</Selected>
          <Hidden>false</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
          <Alignment>None</Alignment>
        </Column>
        <orderBy>ASC</orderBy>
      </OrderByInfo>
      <OrderByInfo>
        <Num>2</Num>
        <Column>
          <ID>36</ID>
          <FieldName>Title</FieldName>
          <DisplayName>Title</DisplayName>
          <DataType>String</DataType>
          <TableName>Services</TableName>
          <Function>none</Function>
          <Sequence>2</Sequence>
          <Selected>true</Selected>
          <Hidden>false</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
          <Alignment>None</Alignment>
        </Column>
        <orderBy>ASC</orderBy>
      </OrderByInfo>
    </OrderBy>
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>Services</Name>
        <Columns>
          <ColumnInfo>
            <ID>14</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>Services</TableName>
            <Sequence>32</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>26</ID>
            <FieldName>ServiceCategoryNameLookup</FieldName>
            <DisplayName>Category Name</DisplayName>
            <DataType>String</DataType>
            <TableName>Services</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>27</ID>
            <FieldName>ServiceDescription</FieldName>
            <DisplayName>Description</DisplayName>
            <DataType>String</DataType>
            <TableName>Services</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>31</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>Services</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>Services.ServiceCategoryNameLookup</ColumnName>
        <DataType>String</DataType>
        <Operator>NotEqual</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>~ModuleAgent~</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
      <WhereInfo>
        <ID>2</ID>
        <RelationOpt>AND</RelationOpt>
        <ColumnName>Services.ServiceCategoryNameLookup</ColumnName>
        <DataType>String</DataType>
        <Operator>NotEqual</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>~ModuleFeedback~</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>Services</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Alert</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 115, 0, 0, N'SMS', N'Accent1', N'Service Counts', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.880' AS DateTime), CAST(N'2018-01-19 12:08:39.880' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (113, NULL, NULL, N'Service Ticket Counts', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Service Ticket Counts</ContainerTitle>
  <Description>SVC Ticket counts by service name</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>94599322-9074-486e-b8ed-87b1a7e2e6a2</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>765595b9-4323-4f2c-9c5f-a123e0ba4120</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Service</Title>
      <SelectedField>ServiceTitleLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>None</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>15</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>sideBySideFullStackedBarValAsPertge</Key>
          <Value xsi:type="xsd:string">True</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title>Count</Title>
      <FactTable />
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle />
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>SVC-OpenTickets</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Nature Colors</Palette>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 5, 480, 360, NULL, N'Accent1', N'Service Ticket Counts', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.880' AS DateTime), CAST(N'2018-01-19 12:08:39.880' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (114, NULL, NULL, N'Application Changes', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Application Changes</ContainerTitle>
  <Description>Shows the list of application changes</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>1fbd816b-85af-40f0-8ce3-c9552d1a171f</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#FFFFFF</BGColor>
  <Id />
  <ChartId>906219a9-5b9c-48ee-994b-562bbe54e3a9</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Applications</Title>
      <SelectedField>TicketRequestTypeCategory</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>sideBySideFullStackedBarValAsPertge</Key>
          <Value xsi:type="xsd:string">True</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title />
      <FactTable />
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle />
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>None</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ACR-OpenTickets</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 6, 800, 360, NULL, N'Accent1', N'Application Changes', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.883' AS DateTime), CAST(N'2018-01-19 12:08:39.883' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (115, NULL, NULL, N'Shows the type of ACRs', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>ACR Categories</ContainerTitle>
  <Description>Shows the type of ACRs</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>689e389f-07b8-4e1b-a72d-b5eadc727344</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#FFFFFF</BGColor>
  <Id />
  <ChartId>d3174bc0-017f-41cd-995f-0828a0bfb4f6</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Departments</Title>
      <SelectedField>ACRTypeTitleLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>None</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>DNLblPercentage</Key>
          <Value xsi:type="xsd:string">True</Value>
        </DataItem>
        <DataItem>
          <Key>DNHoleRadius</Key>
          <Value xsi:type="xsd:string">60</Value>
        </DataItem>
        <DataItem>
          <Key>DNLblPosition</Key>
          <Value xsi:type="xsd:string">Radial</Value>
        </DataItem>
        <DataItem>
          <Key>DNExplodedpoint</Key>
          <Value xsi:type="xsd:string">None</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title>Counts</Title>
      <FactTable />
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Doughnut</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle />
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ACR-OpenTickets</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Mixed</Palette>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, 1, NULL, 7, 360, 360, NULL, N'Accent1', N'ACR Categories', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.883' AS DateTime), CAST(N'2018-01-19 12:08:39.883' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (116, NULL, NULL, N'NPRs by Project Type', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>New Project Requests by Project Type</ContainerTitle>
  <Description>NPRs by Project Type</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>f0cf6db5-fe07-4504-ab55-9def41d0f8de</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#FFFFFF</BGColor>
  <Id />
  <ChartId>fdbbd6ab-61b7-4aaf-ad93-663fe5d33562</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>By Project Type</Title>
      <SelectedField>TicketRequestTypeLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>1</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>true</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>20</AxisLabelMaxLength>
      <LegendTxtMaxLength>20</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>sideBySideFullStackedBarValAsPertge</Key>
          <Value xsi:type="xsd:string">True</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title># of NPRs</Title>
      <FactTable />
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle />
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>NPR-OpenTickets</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>LeftOutside</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>TopToBottom</Direction>
  <MaxHorizontalPercentage>50</MaxHorizontalPercentage>
  <MaxVerticalPercentage>50</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 8, 480, 360, NULL, N'Accent1', N'New Project Requests by Project Type', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.883' AS DateTime), CAST(N'2018-01-19 12:08:39.883' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (117, NULL, NULL, N'New Project Requests by Stage', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>New Project Requests by Stage</ContainerTitle>
  <Description>NPRs by Stage</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>a5bff6b5-f10d-4e7f-a0e6-4937fbb3b05c</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#FFFFFF</BGColor>
  <Id />
  <ChartId>5abce747-b861-4ee5-8367-fd6073932993</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>By Stage</Title>
      <SelectedField>ModuleStepLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>1</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>true</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>None</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>30</AxisLabelMaxLength>
      <LegendTxtMaxLength>25</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>sideBySideFullStackedBarValAsPertge</Key>
          <Value xsi:type="xsd:string">True</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title># of NPRs</Title>
      <FactTable />
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle />
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>NPR-OpenTickets</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Nature Colors</Palette>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 9, 480, 360, NULL, N'Accent1', N'New Project Requests by Stage', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.887' AS DateTime), CAST(N'2018-01-19 12:08:39.887' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (118, NULL, N'none', N'This identifies the DRQs from last quarter (uses open DRQs, but should be both).', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>46756d99-5035-4763-8a13-4f020e242c17</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>46756d99-5035-4763-8a13-4f020e242c17</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy>
      <GroupByInfo>
        <Column>
          <ID>1</ID>
          <FieldName>ActualCompletionDate</FieldName>
          <DisplayName>Actual</DisplayName>
          <DataType>DateTime</DataType>
          <TableName>DRQ-OpenTickets</TableName>
          <Function>none</Function>
          <Sequence>2</Sequence>
          <Selected>false</Selected>
          <Hidden>true</Hidden>
          <IsExpression>false</IsExpression>
          <IsDrillDown>false</IsDrillDown>
          <IsFormattedColumn>false</IsFormattedColumn>
        </Column>
        <Num>1</Num>
      </GroupByInfo>
    </GroupBy>
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DRQ-OpenTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>59</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>Count</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>111</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>TicketId</DisplayName>
            <DataType>String</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Sequence>118</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>1</ID>
            <FieldName>ActualCompletionDate</FieldName>
            <DisplayName>Actual</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>117</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>DRQ-OpenTickets.TicketTargetCompletionDate</ColumnName>
        <DataType>DateTime</DataType>
        <Operator>LessThanEqualTo</Operator>
        <Valuetype>Variable</Valuetype>
        <ParameterType />
        <Value>f:adddays([$Today$],-84)</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DRQ-OpenTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>59</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>String</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>12</ID>
            <FieldName>TicketCreationDate</FieldName>
            <DisplayName>Ticket Creation Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>24</ID>
            <FieldName>TicketTargetCompletionDate</FieldName>
            <DisplayName>Ticket Target Completion Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>112</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>40</ID>
            <FieldName>DepartmentLookup</FieldName>
            <DisplayName>Department Lookup</DisplayName>
            <DataType>String</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text>Deployments Last Quarter</Text>
      <TextFontName>Segoe UI</TextFontName>
      <TextFontStyle>Regular</TextFontStyle>
      <TextFontSize>18pt</TextFontSize>
      <TextForeColor>00ac65</TextForeColor>
      <HideText>false</HideText>
      <Label>Dummy Text</Label>
      <LabelFontName>Times New Roman</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>8pt</LabelFontSize>
      <LabelForeColor>000000</LabelForeColor>
      <HideLabel>true</HideLabel>
      <ResultFontName>Segoe UI</ResultFontName>
      <ResultFontStyle>Regular</ResultFontStyle>
      <ResultFontSize>38pt</ResultFontSize>
      <ResultForeColor>00ac65</ResultForeColor>
      <TitlePosition>Top</TitlePosition>
      <FormatType>FormattedNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>400</Width>
        <Height>240</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithIconAndNoBorder</ResultPanelDesign>
      <IconImage>/_layouts/15/images/ugovernit/uploadedfiles/Greedn Dots.jpg</IconImage>
      <IconLocation>
        <X>116</X>
        <Y>40</Y>
      </IconLocation>
      <IconSize>
        <Width>200</Width>
        <Height>120</Height>
      </IconSize>
      <EnableEditUrl>false</EnableEditUrl>
      <BackgroundColor>ffffff</BackgroundColor>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>Default Drill down</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <Header>DRQs Last Quarter</Header>
      <Footer />
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <AdditionalInfo />
      <AdditionalFooterInfo>Aug-17-2017</AdditionalFooterInfo>
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>false</IsTransparent>
    </QueryFormats>
    <IsPreviewFormatted>true</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Alert</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', N'', 2, N'', N'', 1, 0, 0, 0, 7, 0, 0, N'SMS', N'Accent1', N'DRQs Last Quarter', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.887' AS DateTime), CAST(N'2018-01-19 12:08:39.887' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (119, NULL, NULL, N'Current DRQs', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>3ba43a6e-0adf-4375-bf18-ddfd7402c3c3</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>3ba43a6e-0adf-4375-bf18-ddfd7402c3c3</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>ACR-ClosedTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>9</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>ACR-ClosedTickets</TableName>
            <Sequence>33</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses />
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>ACR-ClosedTickets</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Alert</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 111, 0, 0, NULL, N'Accent1', N'Current DRQs', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.890' AS DateTime), CAST(N'2018-01-19 12:08:39.890' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (120, NULL, NULL, N'Current DRQs', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>09d052e5-8deb-49c2-b67d-19c01b074416</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <QueryId>09d052e5-8deb-49c2-b67d-19c01b074416</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DRQ-OpenTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>6</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>Count</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>DRQ-OpenTickets.TicketTargetCompletionDate</ColumnName>
        <DataType>DateTime</DataType>
        <Operator>GreaterThan</Operator>
        <Valuetype>Variable</Valuetype>
        <ParameterType />
        <Value>f:adddays([$Today$],0)</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DRQ-OpenTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>6</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>String</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>12</ID>
            <FieldName>TicketCreationDate</FieldName>
            <DisplayName>Ticket Creation Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>24</ID>
            <FieldName>TicketTargetCompletionDate</FieldName>
            <DisplayName>Ticket Target Completion Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>25</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text>Upcoming Deployments</Text>
      <TextFontName>Segoe UI</TextFontName>
      <TextFontStyle>Regular</TextFontStyle>
      <TextFontSize>18pt</TextFontSize>
      <TextForeColor>748ca3</TextForeColor>
      <HideText>false</HideText>
      <Label>Dummy Label</Label>
      <LabelFontName>Segoe UI</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>8pt</LabelFontSize>
      <LabelForeColor>000000</LabelForeColor>
      <HideLabel>true</HideLabel>
      <ResultFontName>Segoe UI</ResultFontName>
      <ResultFontStyle>Regular</ResultFontStyle>
      <ResultFontSize>38pt</ResultFontSize>
      <ResultForeColor>748ca3</ResultForeColor>
      <TitlePosition>Top</TitlePosition>
      <FormatType>FormattedNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>400</Width>
        <Height>240</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithIconAndNoBorder</ResultPanelDesign>
      <IconImage>/_layouts/15/images/ugovernit/uploadedfiles/Grey Dots.jpg</IconImage>
      <IconLocation>
        <X>136</X>
        <Y>40</Y>
      </IconLocation>
      <IconSize>
        <Width>200</Width>
        <Height>120</Height>
      </IconSize>
      <BackgroundColor>ffffff</BackgroundColor>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>Default Drill down</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <Header>Current DRQs</Header>
      <Footer />
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <AdditionalInfo />
      <AdditionalFooterInfo>Aug-17-2017</AdditionalFooterInfo>
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>false</IsTransparent>
      <EnableEditUrl>false</EnableEditUrl>
    </QueryFormats>
    <IsPreviewFormatted>true</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Alert</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, 1, NULL, NULL, NULL, 117, 0, 0, NULL, N'Accent1', N'Current DRQs', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:39.890' AS DateTime), CAST(N'2018-01-19 12:08:39.890' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (121, NULL, N'', N'Project Consumers', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Project Consumers</ContainerTitle>
  <Description>Project Consumers</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>40eb088c-1cdc-4288-8739-bc57cca37534</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>35d15bc0-4a11-40ab-a635-755f9359c18a</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Consumer</Title>
      <SelectedField>ProjectClassLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>sideBySideFullStackedBarValAsPertge</Key>
          <Value xsi:type="xsd:string">True</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title />
      <FactTable />
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle />
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>None</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>ACR-ClosedTickets</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Mixed</Palette>
  <BGColor>#E7EAFE</BGColor>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 1, 0, 9, 480, 360, N'', N'Accent1', N'Project Consumers', NULL, NULL, N'', CAST(N'2018-01-19 12:08:39.890' AS DateTime), CAST(N'2018-01-19 12:08:39.890' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (122, NULL, NULL, N'Identifies the current and upcoming deployments', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>02c4ac4b-91af-4b4e-a277-ad2a40938f43</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <BGColor>#E7EAFE</BGColor>
  <Id />
  <QueryId>02c4ac4b-91af-4b4e-a277-ad2a40938f43</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DRQ-OpenTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>6</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>Count</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>DRQ-OpenTickets.TicketTargetCompletionDate</ColumnName>
        <DataType>DateTime</DataType>
        <Operator>GreaterThan</Operator>
        <Valuetype>Variable</Valuetype>
        <ParameterType />
        <Value>f:adddays([$Today$],0)</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DRQ-OpenTickets</Name>
        <Columns>
          <ColumnInfo>
            <ID>6</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>String</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>12</ID>
            <FieldName>TicketCreationDate</FieldName>
            <DisplayName>Ticket Creation Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>2</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>24</ID>
            <FieldName>TicketTargetCompletionDate</FieldName>
            <DisplayName>Ticket Target Completion Date</DisplayName>
            <DataType>DateTime</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>3</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>25</ID>
            <FieldName>Title</FieldName>
            <DisplayName>Title</DisplayName>
            <DataType>String</DataType>
            <TableName>DRQ-OpenTickets</TableName>
            <Function>none</Function>
            <Sequence>4</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>true</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </DrillDownTables>
    <QueryFormats>
      <Text>Future Deployments</Text>
      <TextFontName>Segoe UI</TextFontName>
      <TextFontStyle>Regular</TextFontStyle>
      <TextFontSize>18pt</TextFontSize>
      <TextForeColor>748ca3</TextForeColor>
      <HideText>false</HideText>
      <Label>Dummy Label</Label>
      <LabelFontName>Times New Roman</LabelFontName>
      <LabelFontStyle>Bold</LabelFontStyle>
      <LabelFontSize>8pt</LabelFontSize>
      <LabelForeColor>000000</LabelForeColor>
      <HideLabel>true</HideLabel>
      <ResultFontName>Segoe UI</ResultFontName>
      <ResultFontStyle>Regular</ResultFontStyle>
      <ResultFontSize>38pt</ResultFontSize>
      <ResultForeColor>748ca3</ResultForeColor>
      <TitlePosition>Top</TitlePosition>
      <FormatType>FormattedNumber</FormatType>
      <BackgroundImage />
      <SizeOfFrame>
        <Width>400</Width>
        <Height>240</Height>
      </SizeOfFrame>
      <Location>
        <X>0</X>
        <Y>0</Y>
      </Location>
      <ResultPanelDesign>WithIconAndNoBorder</ResultPanelDesign>
      <IconImage>/_layouts/15/images/ugovernit/uploadedfiles/Grey Dots.jpg</IconImage>
      <IconLocation>
        <X>148</X>
        <Y>40</Y>
      </IconLocation>
      <IconSize>
        <Width>200</Width>
        <Height>120</Height>
      </IconSize>
      <BackgroundColor>ffffff</BackgroundColor>
      <BorderColor>ffffff</BorderColor>
      <BorderWidth>0</BorderWidth>
      <HeaderColor>ffffff</HeaderColor>
      <RowColor>ffffff</RowColor>
      <RowAlternateColor>ffffff</RowAlternateColor>
      <DrillDownType>Default Drill down</DrillDownType>
      <NavigateType>Popup</NavigateType>
      <QueryId>21</QueryId>
      <Header>Future Deployments</Header>
      <Footer />
      <ShowCompanyLogo>false</ShowCompanyLogo>
      <AdditionalInfo />
      <AdditionalFooterInfo>Aug-17-2017</AdditionalFooterInfo>
      <ShowDateInFooter>false</ShowDateInFooter>
      <IsTransparent>false</IsTransparent>
      <EnableEditUrl>false</EnableEditUrl>
    </QueryFormats>
    <IsPreviewFormatted>true</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
    <ParameterList />
  </QueryInfo>
  <ScheduleActionValue>
    <ScheduleId>0</ScheduleId>
    <StartTime>0001-01-01T00:00:00</StartTime>
    <ActionType>Alert</ActionType>
    <Recurring>false</Recurring>
    <RecurringInterval xsi:nil="true" />
    <RecurringEndDate xsi:nil="true" />
    <IsEnable>false</IsEnable>
  </ScheduleActionValue>
</DashboardQuery>', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, 118, 0, 0, NULL, N'Accent1', N'Future Deployments', NULL, NULL, NULL, CAST(N'2018-01-19 12:08:40.043' AS DateTime), CAST(N'2018-01-19 12:08:40.043' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (123, NULL, N'', N'Projects by Project Type', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Projects by Project Type</ContainerTitle>
  <Description>Projects by Project Type</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>b0b9f0ab-6990-4bcb-a59a-376df4a721f5</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>9bb01de1-d82c-41d4-a9d5-4eb0df58f159</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>Project Type</Title>
      <SelectedField>RequestTypeLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>1</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>true</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>sideBySideFullStackedBarValAsPertge</Key>
          <Value xsi:type="xsd:string">True</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title># of Projects</Title>
      <FactTable />
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>0</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle />
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>PMM-OpenTickets</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>true</HideGrid>
  <HideLegend>true</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <BGColor>#E7EAFE</BGColor>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 9, 480, 360, N'', N'Accent1', N'Projects by Project Type', NULL, NULL, N'', CAST(N'2018-01-19 12:08:40.043' AS DateTime), CAST(N'2018-01-19 12:08:40.043' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (124, NULL, N'', N'Assets Summary', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>Assets Summary</ContainerTitle>
  <Description>Assets Summary</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>afccc2b3-2798-40db-9b6a-8eef54e1e9be</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>fee6bc4b-feef-4194-9f41-eefdd682cdbf</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions />
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>sideBySideFullStackedBarValAsPertge</Key>
          <Value xsi:type="xsd:string">True</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title># of Assets</Title>
      <FactTable />
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle />
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <FunctionExpression>ID</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>CMDB-OpenTickets</FactTable>
  <BasicDateFitlerStartField />
  <BasicDateFitlerEndField />
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <BGColor>#E7EAFE</BGColor>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>true</IsCacheChart>
  <CacheSchedule>60</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 113, 280, 640, N'', N'Accent1', N'Assets Summary', NULL, NULL, N'', CAST(N'2018-01-19 12:08:40.043' AS DateTime), CAST(N'2018-01-19 12:08:40.043' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (125, NULL, N'', N'', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>jdsf</ContainerTitle>
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>a47807f6-7324-4a66-a295-15e60bf73251</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>AllOpenTickets</DashboardTable>
      <LinkID>29426eb8-ef0e-4093-9f6a-5b4867ba8d41</LinkID>
      <Title>TItt</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>true</UseAsPanel>
      <ExpressionFormat>Total Open Tickets : $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[TicketId]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit>%</BarUnit>
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category>MyDashboard</ShowColumns_Category>
    </DashboardPanelLink>
  </Expressions>
  <PanelID>3152609b-24c7-4d17-8ab3-126fdf4468a2</PanelID>
  <IconUrl />
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', N'', 0, N'', N'', 0, 0, 0, 0, 119, 300, 300, N'', N'Accent1', N'jdsf', NULL, NULL, N'', CAST(N'2018-01-19 12:08:40.047' AS DateTime), CAST(N'2018-01-19 12:08:40.047' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (126, NULL, N'', N'', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<ChartSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Chart</type>
  <ContainerTitle>TSR TEST $Date$</ContainerTitle>
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>97bb587f-1356-451c-9b61-b912f7ad5230</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Id />
  <ChartId>ea186a47-6872-43b7-aa0f-835a41a71481</ChartId>
  <IsComulative>false</IsComulative>
  <Dimensions>
    <ChartDimension>
      <Title>MANAGER</Title>
      <SelectedField>RequestTypeLookup</SelectedField>
      <Sequence>1</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>NextDimension</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>75</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
    <ChartDimension>
      <Title>Priority</Title>
      <SelectedField>PriorityLookup</SelectedField>
      <Sequence>2</Sequence>
      <PickTopDataPoint>0</PickTopDataPoint>
      <DataPointOrder>descending</DataPointOrder>
      <DataPointExpression>0</DataPointExpression>
      <OrderByExpression>0</OrderByExpression>
      <OrderBy>ascending</OrderBy>
      <EnableSorting>false</EnableSorting>
      <Operator>count</Operator>
      <OperatorField />
      <IsCumulative>false</IsCumulative>
      <FactTable />
      <FilterID>0</FilterID>
      <DataPointClickEvent>Detail</DataPointClickEvent>
      <DateViewType />
      <ShowInDropDown>false</ShowInDropDown>
      <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LegendTxtMaxLength>0</LegendTxtMaxLength>
      <ScaleType>Auto</ScaleType>
    </ChartDimension>
  </Dimensions>
  <Expressions>
    <ChartExpression>
      <ChartLevelProperties>
        <DataItem>
          <Key>sideBySideFullStackedBarValAsPertge</Key>
          <Value xsi:type="xsd:string">True</Value>
        </DataItem>
      </ChartLevelProperties>
      <Title>MANAGE</Title>
      <FactTable />
      <GroupByField />
      <Operator>count</Operator>
      <ExpressionFormula />
      <Order>1</Order>
      <ChartType>Column</ChartType>
      <ShowInPercentage>false</ShowInPercentage>
      <LabelStyle />
      <LabelText />
      <YAsixType>Primary</YAsixType>
      <DataPointClickEvent>Inherit</DataPointClickEvent>
      <FunctionExpression>BusinessManager</FunctionExpression>
      <Palette />
      <DrawingStyle>Default</DrawingStyle>
      <LabelColor>#000000</LabelColor>
      <IsCurrency>false</IsCurrency>
      <Dimensions />
      <HideLabel>false</HideLabel>
      <AxisLabelMaxLength>0</AxisLabelMaxLength>
      <LabelFormat />
    </ChartExpression>
  </Expressions>
  <FactTable>DashboardSummary</FactTable>
  <BasicDateFitlerStartField>CreationDate</BasicDateFitlerStartField>
  <BasicDateFitlerEndField>CreationDate</BasicDateFitlerEndField>
  <BasicDateFilterDefaultView />
  <BasicFilter />
  <HideDateFilterDropdown>false</HideDateFilterDropdown>
  <HideGrid>false</HideGrid>
  <HideLegend>false</HideLegend>
  <HorizontalAlignment>Center</HorizontalAlignment>
  <VerticalAlignment>TopOutside</VerticalAlignment>
  <Direction>LeftToRight</Direction>
  <MaxHorizontalPercentage>100</MaxHorizontalPercentage>
  <MaxVerticalPercentage>100</MaxVerticalPercentage>
  <Palette>Default</Palette>
  <BGColor>#E7EAFE</BGColor>
  <BorderWidth>0</BorderWidth>
  <HideLabel>false</HideLabel>
  <LabelStyle />
  <LabelText />
  <AxisLabelStyleAngle>0</AxisLabelStyleAngle>
  <IsCacheChart>false</IsCacheChart>
  <CacheSchedule>0</CacheSchedule>
  <ReversePlotting>false</ReversePlotting>
</ChartSetting>', N'', 1, N'', N'', 0, 0, 0, 0, 108, 800, 400, N'', N'Accent1', N'TSR TEST $Date$', NULL, NULL, N'', CAST(N'2018-01-19 12:08:40.047' AS DateTime), CAST(N'2018-01-19 12:08:40.047' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (127, NULL, N'undefined', N'', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<DashboardQuery xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle />
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>5b092739-6fd1-40d7-8289-733d0a5d5709</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <QueryId>00000000-0000-0000-0000-000000000000</QueryId>
  <QueryTable />
  <QueryInfo>
    <JoinList />
    <GroupBy />
    <OrderBy />
    <Tables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns>
          <ColumnInfo>
            <ID>0</ID>
            <FieldName>ID</FieldName>
            <DisplayName>ID</DisplayName>
            <DataType>Integer</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>0</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
          <ColumnInfo>
            <ID>6</ID>
            <FieldName>Closed</FieldName>
            <DisplayName>Closed</DisplayName>
            <DataType>Boolean</DataType>
            <TableName>DashboardSummary</TableName>
            <Function>Count</Function>
            <Sequence>1</Sequence>
            <Selected>true</Selected>
            <Hidden>false</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
            <Alignment>None</Alignment>
          </ColumnInfo>
          <ColumnInfo>
            <ID>46</ID>
            <FieldName>TicketId</FieldName>
            <DisplayName>TicketId</DisplayName>
            <DataType>String</DataType>
            <TableName>DashboardSummary</TableName>
            <Sequence>48</Sequence>
            <Selected>false</Selected>
            <Hidden>true</Hidden>
            <IsExpression>false</IsExpression>
            <IsDrillDown>false</IsDrillDown>
            <IsFormattedColumn>false</IsFormattedColumn>
          </ColumnInfo>
        </Columns>
      </TableInfo>
    </Tables>
    <WhereClauses>
      <WhereInfo>
        <ID>1</ID>
        <RelationOpt>None</RelationOpt>
        <ColumnName>DashboardSummary.Closed</ColumnName>
        <DataType>Boolean</DataType>
        <Operator>Equal</Operator>
        <Valuetype>Constant</Valuetype>
        <ParameterType />
        <Value>True</Value>
        <ParameterName />
        <ParameterRequired>true</ParameterRequired>
      </WhereInfo>
    </WhereClauses>
    <Totals />
    <DrillDownTables>
      <TableInfo>
        <ID>1</ID>
        <Name>DashboardSummary</Name>
        <Columns />
      </TableInfo>
    </DrillDownTables>
    <IsPreviewFormatted>false</IsPreviewFormatted>
    <IsGroupByExpanded>false</IsGroupByExpanded>
  </QueryInfo>
</DashboardQuery>', N'', 2, N'', N'', 0, 0, 0, 0, 122, 0, 0, N'', N'Accent1', N'Close Tickets', NULL, NULL, N'', CAST(N'2018-01-19 12:08:40.047' AS DateTime), CAST(N'2018-01-19 12:08:40.047' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (128, NULL, N'', N'Closed Assets More Than 4 Years', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>Closed Assets More Than 4 Years</ContainerTitle>
  <Description>Closed Assets More Than 4 Years</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>2a393677-2a1c-49ad-a4d4-60686a231da3</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>CMDB-ClosedTickets</DashboardTable>
      <LinkID>7bbd9d46-a92e-481b-9b45-538df2213df5</LinkID>
      <Title>PC-Test</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>1</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>Total Tickets : $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>f:YearsDiff(Startdate,RetiredDate)&gt;4</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>27871c8a-6796-45c7-ad7d-bc3f3dda082b</PanelID>
  <IconUrl />
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', N'', 0, N'', N'', 0, 0, 0, 0, 123, 300, 300, N'', N'Accent1', N'Closed Assets More Than 4 Years', NULL, NULL, N'', CAST(N'2018-01-19 12:08:40.047' AS DateTime), CAST(N'2018-01-19 12:08:40.047' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (129, NULL, N'', N'', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>hskhfdf</ContainerTitle>
  <Description />
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>c32d5550-7c83-48c5-afb9-bca0ff8927ec</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>ACR-OpenTickets</DashboardTable>
      <LinkID>7566cc11-5f99-40bc-83d1-59433cf88afe</LinkID>
      <Title>tettt</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>0</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>$exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>[ID]</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>c4cd23a4-5754-423e-83ef-54acbfdb832e</PanelID>
  <IconUrl />
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', N'', 0, N'', N'', 0, 0, 0, 0, 126, 300, 300, N'', N'Accent1', N'hskhfdf', NULL, NULL, N'', CAST(N'2018-01-19 12:08:40.047' AS DateTime), CAST(N'2018-01-19 12:08:40.047' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[Config_Dashboard_DashboardPanels] ([ID], [AuthorizedToView], [CategoryName], [DashboardDescription], [DashboardModuleMultiLookup], [DashboardPanelInfo], [DashboardPermission], [DashboardType], [FontStyle], [HeaderFontStyle], [IsActivated], [IsHideDescription], [IsHideTitle], [IsShowInSideBar], [ItemOrder], [PanelHeight], [PanelWidth], [SubCategory], [ThemeColor], [Title], [Attachments], [TenantID], [Icon], [Created], [Modified], [CreatedBy], [ModifiedBy], [Deleted]) VALUES (130, NULL, N'', N'sfdf', NULL, N'<?xml version="1.0" encoding="utf-16"?>
<PanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <type>Panel</type>
  <ContainerTitle>Open Assets More Than 4 Years</ContainerTitle>
  <Description>sfdf</Description>
  <CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn>
  <Order>0</Order>
  <DashboardID>64379e8c-b3b5-4936-980a-c39189844ee8</DashboardID>
  <HideZoomView>false</HideZoomView>
  <HideTableView>false</HideTableView>
  <HidewDownloadView>false</HidewDownloadView>
  <StartFromNewLine>false</StartFromNewLine>
  <Expressions>
    <DashboardPanelLink>
      <PanelModuleType>All</PanelModuleType>
      <ExpressionID>0</ExpressionID>
      <FormulaId>0</FormulaId>
      <ViewType>0</ViewType>
      <ModuleName />
      <DashboardTable>CMDB-OpenTickets</DashboardTable>
      <LinkID>d73317fc-3d8c-40b5-96a8-b1b80a79e3a9</LinkID>
      <Title>Tesdffd</Title>
      <LinkUrl />
      <DefaultLink>true</DefaultLink>
      <ScreenView>1</ScreenView>
      <IsHide>false</IsHide>
      <Order>10</Order>
      <UseAsPanel>false</UseAsPanel>
      <ExpressionFormat>More Than 4 Year Closed Aseets: $exp$</ExpressionFormat>
      <HideTitle>true</HideTitle>
      <DateFilterStartField />
      <DateFilterDefaultView />
      <Filter />
      <MaxLimit>100</MaxLimit>
      <DecimalPoint>0</DecimalPoint>
      <ShowBar>false</ShowBar>
      <AggragateFun>Count</AggragateFun>
      <AggragateOf>f:DaysDiff(StartDate,RetiredDate)&gt;4</AggragateOf>
      <BarDefaultColor>#FF7F7F</BarDefaultColor>
      <IsPct>false</IsPct>
      <Conditions />
      <FontColor>#000000</FontColor>
      <BarUnit />
      <StopLinkDetail>false</StopLinkDetail>
      <ShowColumns_Category />
    </DashboardPanelLink>
  </Expressions>
  <PanelID>f168c2f3-954c-403f-8200-5c0d92b2a6a7</PanelID>
  <IconUrl />
  <ColumnViewType>0</ColumnViewType>
  <StopAutoScale>false</StopAutoScale>
</PanelSetting>', N'', 0, N'', N'', 0, 0, 0, 0, 125, 300, 300, N'', N'Accent1', N'Open Assets More Than 4 Years', NULL, NULL, N'', CAST(N'2018-01-19 12:08:40.047' AS DateTime), CAST(N'2018-01-19 12:08:40.047' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0)
SET IDENTITY_INSERT [dbo].[Config_Dashboard_DashboardPanels] OFF