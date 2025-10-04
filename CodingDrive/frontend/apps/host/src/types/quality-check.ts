// AI程式碼品質檢查相關型別定義
export interface QualityIssueType {
  id: string;
  ruleId: string;
  severity: 'Critical' | 'High' | 'Medium' | 'Low' | 'Info';
  category: 'CodeStyle' | 'Complexity' | 'Security' | 'Performance' | 'Maintainability' | 'Reliability' | 'TestCoverage';
  title: string;
  description: string;
  fileName: string;
  lineNumber: number;
  columnNumber?: number;
  codeSnippet: string;
  suggestion: string;
  isFixed: boolean;
  detectedAt: Date;
}

export interface CodeQualityCheckType {
  id: string;
  projectId: string;
  commitHash: string;
  branchName: string;
  trigger: 'Manual' | 'CommitPush' | 'PullRequest' | 'Scheduled' | 'API';
  status: 'Pending' | 'Running' | 'Completed' | 'Failed' | 'Cancelled';
  startedAt: Date;
  completedAt?: Date;
  overallScore: {
    score: number;
    codeStyle: number;
    complexity: number;
    security: number;
    performance: number;
    testCoverage: number;
    grade: 'A' | 'B' | 'C' | 'D' | 'F';
  };
  issues: QualityIssueType[];
  metrics: {
    type: string;
    name: string;
    value: number;
    unit: string;
    trend?: 'Improving' | 'Declining' | 'Stable';
  }[];
  requestedBy: string;
}

export interface QualityCheckStartParams {
  projectId: string;
  commitHash: string;
  branchName: string;
  trigger: CodeQualityCheckType['trigger'];
  configuration?: {
    enabledRules?: string[];
    disabledRules?: string[];
    includeTestFiles?: boolean;
    excludedPaths?: string[];
  };
}