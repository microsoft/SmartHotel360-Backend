{{- define "postgres-name" -}}
{{- if .Values.pg.host -}}
{{- .Values.pg.host -}}
{{- else -}}
{{- printf "%s-%s-%s" "reviews" "sh360-postgres" .Release.Name | -}}
{{- end -}}
{{- end -}}
