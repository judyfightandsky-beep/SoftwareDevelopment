import { BaseEntity } from './common'

export enum CheckTrigger {
  Manual = 'Manual',
  CommitPush = 'CommitPush',
  PullRequest = 'PullRequest',
  Scheduled = 'Scheduled',
  API = 'API'
}

export enum CheckStatus {
  Pending = 'Pending',
  Running = 'Running',
  Completed = 'Completed',
  Failed = 'Failed',
  Cancelled = 'Cancelled'
}

export enum IssueSeverity {
  Critical = 'Critical',
  High = 'High',
  Medium = 'Medium',
  Low = 'Low',
  Info = 'Info'
}

export enum IssueCategory {
  CodeStyle = 'CodeStyle',
  Complexity = 'Complexity',
  Security = 'Security',
  Performance = 'Performance',
  Maintainability = 'Maintainability',
  Reliability = 'Reliability',
  TestCoverage = 'TestCoverage'
}

export enum ScoreGrade {
  A = 'A',
  B = 'B',
  C = 'C',
  D = 'D',
  F = 'F'
}

export interface CodeQualityCheck extends BaseEntity {
  projectId: string
  commitHash: string
  branchName: string
  triggerType: CheckTrigger
  status: CheckStatus
  startedAt: Date
  completedAt?: Date
  overallScore: QualityScore
  issues: QualityIssue[]
  metrics: CodeMetric[]
  configuration: CheckConfiguration
}

export interface QualityScore {
  overallScore: number
  codeStyleScore: number
  complexityScore: number
  securityScore: number
  performanceScore: number
  testCoverageScore: number
  grade: ScoreGrade
}

export interface QualityIssue {
  id: string
  ruleId: string
  severity: IssueSeverity
  category: IssueCategory
  title: string
  description: string
  fileName: string
  lineNumber: number
  columnNumber?: number
  codeSnippet: string
  suggestion: string
  isFixed: boolean
  detectedAt: Date
}

export interface CodeMetric {
  type: string
  name: string
  value: number
  unit: string
  trend: 'Improving' | 'Declining' | 'Stable'
  thresholdValue?: number
  isWithinThreshold: boolean
}

export interface CheckConfiguration {
  enabledRules: string[]
  disabledRules: string[]
  ruleParameters: Record<string, unknown>
  thresholds: QualityThresholds
  includeTestFiles: boolean
  excludedPaths: string[]
}

export interface QualityThresholds {
  overallScore: number
  complexity: number
  security: number
  testCoverage: number
}