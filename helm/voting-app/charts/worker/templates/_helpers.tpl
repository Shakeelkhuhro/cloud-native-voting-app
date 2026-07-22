{{/*
Chart name
*/}}

{{- define "worker.name" -}}
{{- .Chart.Name -}}
{{- end }}


{{/*
Full resource name
*/}}

{{- define "worker.fullname" -}}
{{- printf "%s-%s" .Release.Name .Chart.Name -}}
{{- end }}


{{/*
Common labels
*/}}

{{- define "worker.labels" -}}

app.kubernetes.io/name: {{ include "worker.name" . }}

app.kubernetes.io/instance: {{ .Release.Name }}

app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}

app.kubernetes.io/managed-by: {{ .Release.Service }}

helm.sh/chart: {{ printf "%s-%s" .Chart.Name .Chart.Version }}

{{- end }}


{{/*
Selector labels
*/}}

{{- define "worker.selectorLabels" -}}

app: worker

{{- end }}

