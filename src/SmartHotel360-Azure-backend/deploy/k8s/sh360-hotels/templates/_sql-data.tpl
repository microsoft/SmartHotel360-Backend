{{- define "sql-name" -}}
{{- if .Values.sql.host -}}
{{- .Values.sql.host -}}
{{- else -}}
{{- printf "%s-%s" "sh360-sql-data" .Release.Name | -}}
{{- end -}}
{{- end -}}
