{{/*
Expand the chart name.
*/}}
{{- define "vote.name" -}}
{{- .Chart.Name -}}
{{- end }}

{{/*
Create a fullname using the release name.
*/}}
{{- define "vote.fullname" -}}
{{- printf "%s-%s" .Release.Name .Chart.Name -}}
{{- end }}

{{/*
Common labels.
*/}}
{{- define "vote.labels" -}}
app.kubernetes.io/name: {{ include "vote.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
helm.sh/chart: {{ printf "%s-%s" .Chart.Name .Chart.Version }}
{{- end }}

{{/*
Selector labels.
*/}}
{{- define "vote.selectorLabels" -}}
app.kubernetes.io/name: {{ include "vote.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}
