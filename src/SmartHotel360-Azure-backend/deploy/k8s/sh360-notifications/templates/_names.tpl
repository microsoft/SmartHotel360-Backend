{{- define "suffix-name" -}}
{{- if .Values.appName -}}
{{- .Values.appName -}}
{{- else -}}
{{- .Release.Name -}}
{{- end -}}
{{- end -}}

{{- define "sql-name" -}}
{{- if .Values.mssql.host -}}
{{- .Values.mssql.host -}}
{{- else -}}
{{- $suffix := include "suffix-name" . -}}
{{- printf "%s-%s" "sh360-sql-data" $suffix | -}}
{{- end -}}
{{- end -}}

{{- define "tasks-postgres-name" -}}
{{- if .Values.pg.host -}}
{{- .Values.pg.host -}}
{{- else -}}
{{- $suffix := include "suffix-name" . -}}
{{- printf "%s-%s-%s" "tasks" "sh360-postgres" $suffix | -}}
{{- end -}}
{{- end -}}

{{- define "reviews-postgres-name" -}}
{{- if .Values.pg.host -}}
{{- .Values.pg.host -}}
{{- else -}}
{{- $suffix := include "suffix-name" . -}}
{{- printf "%s-%s-%s" "reviews" "sh360-postgres" $suffix | -}}
{{- end -}}
{{- end -}}
