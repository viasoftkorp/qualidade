package utils

import (
	"fmt"
	"io"
	"mime/multipart"
	"net/textproto"
	"path/filepath"
	"strings"
)

type IWriterUtils interface {
	CopyFilesByMultipartForm(form multipart.Form) error
}

type WriterUtils struct {
	Writer     *multipart.Writer
	PipeWriter *io.PipeWriter
}

func NewWriterUtils(writer *multipart.Writer, pipeWriter *io.PipeWriter) IWriterUtils {
	return &WriterUtils{
		Writer:     writer,
		PipeWriter: pipeWriter,
	}
}

func (base *WriterUtils) CopyFilesByMultipartForm(form multipart.Form) error {
	var err error

	go func() {
		defer base.PipeWriter.Close()
		defer base.Writer.Close()

		for _, fileHeaders := range form.File {
			err = base.copyFile(fileHeaders[0])
			if err != nil {
				base.PipeWriter.CloseWithError(err)
			}
		}
	}()

	return nil
}
func (base *WriterUtils) copyFile(fileHeader *multipart.FileHeader) error {
	file, err := fileHeader.Open()
	if err != nil {
		return err
	}

	defer file.Close()

	part, err := base.createFormFile("file", filepath.Base(fileHeader.Filename), fileHeader.Header.Get("Content-Type"))
	if err != nil {
		return err
	}

	if _, err = io.Copy(part, file); err != nil {
		return err
	}

	return nil
}

func (base *WriterUtils) createFormFile(fieldname, filename string, contentType string) (io.Writer, error) {
	h := make(textproto.MIMEHeader)
	h.Set("Content-Disposition",
		fmt.Sprintf(`form-data; name="%s"; filename="%s"`,
			escapeQuotes(fieldname), escapeQuotes(filename)))
	h.Set("Content-Type", contentType)
	return base.Writer.CreatePart(h)
}

var quoteEscaper = strings.NewReplacer("\\", "\\\\", `"`, "\\\"")

func escapeQuotes(s string) string {
	return quoteEscaper.Replace(s)
}
